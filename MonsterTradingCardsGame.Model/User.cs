using MTCGame.Model;

namespace MonsterTradingCardsGame.Model
{
    public class User
    {
        public string Username;// { get; set; } //Username acts as Id
        public string? Password;// { get; set; } //private string _password;    //only holds hash values!
        public int? Coins;// { get; set; }       //private int _coins;          //opt. class: wallet
        public List<Card>? Deck;// { get; set; }
        public List<Card>? Stack;// { get; set; } 
        public int? Elo;// { get; set; }
        //todo: revise to set to private and revise to set nonnullable

        public User(string username)
        {
            this.Username = username;
        }
        public override string ToString() { 
            return Username; 
        }

        //battle()
        //trade()
        //buyPackage()


    }
}