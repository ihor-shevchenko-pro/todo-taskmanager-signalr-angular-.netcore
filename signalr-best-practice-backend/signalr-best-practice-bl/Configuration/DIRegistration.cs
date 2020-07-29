using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using signalr_best_practice_bl.Services;
using signalr_best_practice_bl.Services.UserAccount;
using signalr_best_practice_core.Interfaces.Cache;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Repositories;
using signalr_best_practice_core.Interfaces.Repositories.ManyToMany;
using signalr_best_practice_core.Interfaces.Repositories.UserAccount;
using signalr_best_practice_core.Interfaces.Services;
using signalr_best_practice_core.Interfaces.Services.UserAccount;
using signalr_best_practice_core.Managers;
using signalr_best_practice_dl.Repositories;
using signalr_best_practice_dl.Repositories.ManyToMany;
using signalr_best_practice_dl.Repositories.UserAccount;
using signalr_best_practice_signalr.Cache;
using signalr_best_practice_signalr.Managers;

namespace signalr_best_practice_bl.Configuration
{
    public abstract class DIRegistration
    {
        protected IConfiguration Configuration;

        public DIRegistration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public abstract void RegisterConfigs(ref IServiceCollection services);

        public virtual void RegisterAll(ref IServiceCollection services)
        {
            RegisterConfigs(ref services);
            RegisterServices(ref services);
            RegisterRepositories(ref services);
            RegisterManagers(ref services);
            RegisterCache(ref services);
        }

        public virtual void RegisterServices(ref IServiceCollection services)
        {
            // Services
            services.AddTransient<IToDoTaskService, ToDoTaskService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
        }

        public virtual void RegisterRepositories(ref IServiceCollection services)
        {
            // Repositories
            services.AddTransient<IToDoTaskRepository, ToDoTaskRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

            // ManyToMany Repositories
            services.AddTransient<IUserRoleRepository, UserRoleRepository>();
        }

        public virtual void RegisterManagers(ref IServiceCollection services)
        {
            // Managers
            services.AddTransient<IPasswordManager, PasswordManager>();
            services.AddTransient<ISignalRNotificationManager, SignalRNotificationManager>();
            services.AddTransient<IModelTypeManager, ModelTypeManager>();
        }

        public virtual void RegisterCache(ref IServiceCollection services)
        {
            services.AddSingleton<IHubConnectionCache, HubConnectionCache>();
        }
    }
}
