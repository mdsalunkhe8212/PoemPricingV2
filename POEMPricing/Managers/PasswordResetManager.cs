using System;
using System.Security.Cryptography;
using System.Web;
using System.Web.Caching;

namespace POEMPricing.Managers
{
    public class PasswordResetManager
    {
        private const int ExpiryMinutes = 10;

        private string GetCacheKey(string email)
        {
            return "RESET_CODE_" + email.ToLower();
        }

        public string GenerateCode()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];

                rng.GetBytes(bytes);

                int number = Math.Abs(BitConverter.ToInt32(bytes, 0));

                return (number % 900000 + 100000).ToString();
            }
        }

        public void SaveCode(string email, string code)
        {
            HttpRuntime.Cache.Insert(
                GetCacheKey(email),
                code,
                null,
                DateTime.Now.AddMinutes(ExpiryMinutes),
                Cache.NoSlidingExpiration);
        }

        public bool ValidateCode(string email, string code)
        {
            var savedCode = HttpRuntime.Cache.Get(GetCacheKey(email)) as string;

            if (string.IsNullOrEmpty(savedCode))
                return false;

            return savedCode == code;
        }

        public void RemoveCode(string email)
        {
            HttpRuntime.Cache.Remove(GetCacheKey(email));
        }
    }
}