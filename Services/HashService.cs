using System;
using System.Security.Cryptography;
using DotNetEnv;

namespace failure_api.Services
{
    public class HashService : IHashService
    {
        public string HashGoogleId(string id)
        {
            string salt = Env.GetString("GOOGLE_ID_SALT");
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(id + salt));
            return Convert.ToBase64String(bytes);
        }

        public bool VerifyGoogleId(string id, string hash)
        {
            return hash == HashGoogleId(id);
        }
    }
}