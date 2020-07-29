using Microsoft.Extensions.Configuration;
using signalr_best_practice_core.Interfaces.Managers;

namespace signalr_best_practice_dl_postgresql
{
    public class PostgreSqlSettings : IDatabaseSettings
    {
        protected readonly string _connectionString;

        public PostgreSqlSettings()
        {
            _connectionString = "Host=localhost;Port=5432;Database=SignalR_Best_Practice_Dev;Username=postgres;Password=Wego123!";
        }

        public PostgreSqlSettings(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
