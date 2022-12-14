using MonsterTradingCardsGame.DAL;
using MonsterTradingCardsGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BL
{
    public class UserHandler
    {
        public void RegisterUser(User newUser)
        {
            var dal = new PostgreSQLRepository();
            //DBConnection db = new DBConnection();
            //dal.CreateUser(newUser);
        }

        //unregister()

        //changeProfile()
    }
}
