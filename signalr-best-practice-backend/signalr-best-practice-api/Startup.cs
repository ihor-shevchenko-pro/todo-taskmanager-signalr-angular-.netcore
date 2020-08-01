using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using signalr_best_practice_api.Filters;
using signalr_best_practice_api_models.Models.Response;
using signalr_best_practice_bl.Configuration;
using signalr_best_practice_core.Configuration;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Repositories.Base;
using signalr_best_practice_dl_postgresql;
using signalr_best_practice_dl_postgresql.Repositories;
using signalr_best_practice_signalr.Hubs;

namespace signalr_best_practice_api
{
    public class Startup
    {
        private readonly string _webClientUrl;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _webClientUrl = Configuration["HttpClients:WebClientUrl"];
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // JWT
            AddJwtAuthentication(services);

            // SignalR
            services.AddSignalR().AddJsonProtocol(options => {
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            });

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllPolicy",
                    builder =>
                    {
                        builder
                        .WithOrigins(_webClientUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });

            // NewtonJson
            services.AddControllers().AddNewtonsoftJson();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "SignalR-Best-Practice-API",
                    Description = "SignalR-Best-Practice Application",
                });
            });

            WebApiDIRegistration register = new WebApiDIRegistration(Configuration);
            register.RegisterAll(ref services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Global error handler
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            // HTTP error handler
            app.UseStatusCodePages(async context =>
            {
                string message = "One or more errors occurred";

                if (context.HttpContext.Request.Path.StartsWithSegments("/api") &&
                   (context.HttpContext.Response.StatusCode == 401 || context.HttpContext.Response.StatusCode == 403))
                {
                    message = "Unauthorized request";
                }
                else if (context.HttpContext.Response.StatusCode == 404)
                {
                    message = "Not Found";
                }

                var model = ResponseModelBuild(message);
                var json = JsonConvert.SerializeObject(model);
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(json);
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            // CORS
            app.UseCors("AllowAllPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/signalr");
            });
        }

        // JWT
        private void AddJwtAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = AuthJwtConfig.Current.SymmetricSecurityKey,
                    ValidateIssuer = true,
                    ValidIssuer = AuthJwtConfig.Current.Issuer,
                    ValidateAudience = true,
                    ValidAudience = AuthJwtConfig.Current.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/signalr"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }

        // Extra class container for register all DI
        internal class WebApiDIRegistration : DIRegistration
        {
            public WebApiDIRegistration(IConfiguration configuration) : base(configuration)
            {
            }

            public override void RegisterConfigs(ref IServiceCollection services)
            {
                services.AddSingleton(DataMapperConfig.GetMapper());
                services.AddTransient<IDataMapper, DataMapper>();

                services.AddTransient<IDatabaseSettings, PostgreSqlSettings>();
                services.AddTransient(typeof(IGenericRepository<,>), typeof(PostgreSqlRepository<,>));
                services.AddTransient<IDbContext, PostgreSqlContext>();                           
                services.AddSingleton<IDatabaseInitializer, PostgreSqlContextInitializer>();
            }
        }

        private static ErrorResponceApiModel<string> ResponseModelBuild(params string[] errors)
        {
            ErrorResponceApiModel<string> result = new ErrorResponceApiModel<string>()
            {
                Errors = errors.ToList(),
            };

            return result;
        }
    }
}
