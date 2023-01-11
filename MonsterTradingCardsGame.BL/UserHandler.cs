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
            Console.WriteLine("eebydeeby");
            //prep user class
            user.Password = HashPassword(user.Password);
            //save to repo
            repo.CreateUser(user);
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
