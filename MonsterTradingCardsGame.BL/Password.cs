using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.BL
{
    public static class Password
    {
        public static string HashPassword(string password)
        {
            string hash = "";
            //password.GetHashCode().ToString();
            hash = "hash" + password;
            return hash;
        }
    }
}
