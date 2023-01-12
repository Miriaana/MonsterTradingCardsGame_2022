using MonsterTradingCardsGame.DAL;
using MonsterTradingCardsGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MonsterTradingCardsGame.BL
{
    public class UserHandler
    {
        /*
        public void RegisterUser(User newUser)
        {
            var dal = new PostgreSQLRepository();
            //DBConnection db = new DBConnection();
            //dal.CreateUser(newUser);
        }*/

        public void CreateUser(User user)
        {
            var repo = new PostgreSQLRepository();

            user.Password = HashPassword(user.Password);

            repo.CreateUser(user);
        }

        public User GetUser(User user)
        {
            var repo = new PostgreSQLRepository();

            return repo.GetUser(user);
        }

        //UpdateUserProfile
        public void UpdateUserProfile(User user)
        {
            var repo = new PostgreSQLRepository();

            //yeah, but do you update and what do you not
            repo.UpdateProfile(user);
        }

        string HashPassword(string password)
        {
            string hash = password.GetHashCode().ToString();
            return hash;
        }

        //unregister()

        //changeProfile()
    }
}
