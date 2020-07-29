using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using signalr_best_practice_core.Interfaces.Repositories.Base;

namespace signalr_best_practice_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var environment = services.GetRequiredService<IWebHostEnvironment>();

                        var context = services.GetRequiredService<IDatabaseInitializer>();
                        context.Initialize();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while seeding the database.");
                    }
                }

                host.Run();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
