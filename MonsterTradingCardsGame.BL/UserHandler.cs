using MTCGame.DAL;
using MTCGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MTCGame.BL
{
    public class UserHandler
    {
        public void CreateUser(User user)
        {
            var repo = new PostgreSQLRepository();

            user.Password = Password.HashPassword(user.Password);

            repo.CreateUser(user);
        }

        public User GetUser(string mtcgAuth, User user)
        {
            var repo = new PostgreSQLRepository();

            string auth = repo.CheckAuthorization(mtcgAuth);
            if (string.IsNullOrEmpty(auth))
            {
                throw new Exception("401: Access token is missing or invalid");
            }

            return repo.GetUser(user);
        }

        //UpdateUserProfile
        public void UpdateUserProfile(string mtcgAuth, User user)
        {
            var repo = new PostgreSQLRepository();

            string auth = repo.CheckAuthorization(mtcgAuth);
            if (string.IsNullOrEmpty(auth))
            {
                throw new Exception("401: Access token is missing or invalid");
            }

            repo.UpdateProfile(user);
        }
    }
}
