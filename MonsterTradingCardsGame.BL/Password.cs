using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MTCGame.BL
{
    public static class Password
    {
        // source https://www.delftstack.com/howto/csharp/csharp-sha256/
        //internal static string GetStringSHA256(string text)
        public static string HashPassword(string password)
        {
            if (String.IsNullOrEmpty(password))
                return String.Empty;

            using (var sha = new SHA256Managed())
            {
                byte[] textD = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(textD);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}
