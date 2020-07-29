using signalr_best_practice_core.Interfaces.Managers;
using System;
using System.Security.Cryptography;
using System.Text;

namespace signalr_best_practice_core.Managers
{
    public class PasswordManager : IPasswordManager
    {
        public string GetHash(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Password is empty");
            }

            return GetHash(Encoding.ASCII.GetBytes(value));
        }

        private string GetHash(byte[] bytes)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = string.Empty;
            byte[] crypto = crypt.ComputeHash(bytes, 0, bytes.Length);
            return Convert.ToBase64String(crypto);
        }
    }
}
