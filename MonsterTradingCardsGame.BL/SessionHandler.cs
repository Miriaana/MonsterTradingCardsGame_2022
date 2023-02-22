using MTCGame.DAL;
using MTCGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.BL
{
    public class SessionHandler
    {
        public string CreateSession(User user)
        {
            var repo = new PostgreSQLRepository();
            Console.WriteLine($"given pw: {user.Password}");

            user.Password = Password.HashPassword(user.Password);
            Console.WriteLine($"given pw: {user.Password}");

            if (repo.VerifyPassword(user))
            {
                return repo.CreateSession(user);
            }
            else
            {
                throw new Exception("401: Invalid Username or Password");
            }
        }
    }
}
