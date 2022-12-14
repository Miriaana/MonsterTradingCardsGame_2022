namespace MonsterTradingCardsGame.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        //private string _password;    //only holds hash values!
        private int _coins;          //opt. class: wallet

        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
            //this._password = password;
            //this._coins = 20;
        }
        public override string ToString() { 
            return Username; 
        }

        //battle()
        //trade()
        //buyPackage()


    }
}