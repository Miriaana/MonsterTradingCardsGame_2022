using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class User
    {
        public string Username { get; set; }
        private string _password;    //only ever save hash values!
        private int _coins;          //opt. class: wallet

        public User(string username, string password)
        {
            this.Username = username;
            this._password = password;
            this._coins = 20;
        }

        

        //battle()
        //trade()
        //buyPackage()


    }
}
