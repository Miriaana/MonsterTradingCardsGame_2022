namespace MonsterTradingCardsGame.Model
{
    public class User
    {
        public string Name { get; set; }
        private string _password;    //only ever save hash values!
        private int _coins;          //opt. class: wallet

        public User(string username, string password)
        {
            this.Name = username;
            this._password = password;
            this._coins = 20;
        }

        public override string ToString() { 
            return Name; 
        }

        //battle()
        //trade()
        //buyPackage()


    }
}