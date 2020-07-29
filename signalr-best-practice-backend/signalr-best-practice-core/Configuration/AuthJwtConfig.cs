using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace signalr_best_practice_core.Configuration
{
    public class AuthJwtConfig
    {
        private static readonly AuthJwtConfig _instance = new AuthJwtConfig();
        public static AuthJwtConfig Current => _instance;
        public string SecretKey => "mysupersecret_secretkey!123";

        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        public string SigningAlgorithm => SecurityAlgorithms.HmacSha256;

        public string Issuer => "MainAuthServer";
        public string Audience => "https://127.0.0.1:5001";

        private int LifetimeMinutes => 1440;
        public TimeSpan Lifetime => TimeSpan.FromMinutes(LifetimeMinutes);

        private AuthJwtConfig()
        {
        }
    }
}
