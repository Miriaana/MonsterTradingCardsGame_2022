namespace MTCGame.Model
{
    public class User
    {
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
        //public string Username;// { get; set; } //Username acts as Id
        //public string? Password;// { get; set; } //private string _password;    //only holds hash values!
        //ID??
        public int? Coins;// { get; set; }       //private int _coins;          //opt. class: wallet
        public List<Card>? Deck;// { get; set; }
        public List<Card>? Stack;// { get; set; } 
        public int? Elo { get; set; }
        public int? Wins { get; set; }
        public int? Losses { get; set; }
        //todo: revise to set to private and revise to set nonnullable
        public string? Name { get; set; } //Profilename
        public string? Image { get; set; }
        public string? Bio { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
        
        public User(string username)
        {
            Username = username;
        }

        public User()
        {
        }

        public string ShowProfile()
        {
            string line = string.Empty;
            line += $"{Username}'s profile:";
            line += $"\n\tName:  {Name}";
            line += $"\n\tBio:   {Bio}";
            line += $"\n\tImage: {Image}";

            return line;
        }

        public override string ToString() { 
            return Username; 
        }
    }
}