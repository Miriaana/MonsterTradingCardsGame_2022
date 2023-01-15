//using MonsterTradingCardsGame.Model;
using MTCGame.Model;
using Npgsql;
using NpgsqlTypes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace MTCGame.DAL
{
    public class PostgreSQLRepository
    {
        string ConnectionString = "Host=localhost;Username=swe1user;Password=swe1pw;Database=mtcgdb;Pooling=true;Minimum Pool Size=0;Maximum Pool Size=100;";
        public PostgreSQLRepository() {
            
        }
        public void CreateUser(User user)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                /*IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=mtcgdb");
            connection.Open();
            Console.WriteLine($"Connection open");*/
                {
                    IDbCommand command = connection.CreateCommand();
                    command.CommandText = @"
insert into users 
    (Username, Password, Coins, Elo) 
values
    (@USERNAME, @PASSWORD, @COINS, @ELO)
";

                    NpgsqlCommand c = command as NpgsqlCommand;

                    //c.Parameters.Add("UserId", NpgsqlDbType.Integer);
                    c.Parameters.Add("Username", NpgsqlDbType.Varchar, 50);

                    c.Parameters.Add("Password", NpgsqlDbType.Varchar, 50);
                    c.Parameters.Add("Coins", NpgsqlDbType.Integer);
                    c.Parameters.Add("Elo", NpgsqlDbType.Integer);

                    c.Prepare();

                    //c.Parameters["UserId"].Value = 1;
                    c.Parameters["Username"].Value = user.Username;
                    c.Parameters["Password"].Value = user.Password;
                    c.Parameters["Coins"].Value = 20;
                    c.Parameters["Elo"].Value = 0;

                    command.ExecuteNonQuery();
                }
            }
        }

        public User GetUser(string mtcgAuth, User user)
        {
            Console.WriteLine("getting user info");
            return user;
        }

        public void UpdateProfile(string mtcgAuth, User user) 
        {
            Console.WriteLine("updating profile");
        }

        public string CreateSession(User user)
        {
            Console.WriteLine("TODO: create session");
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
insert into sessions 
    (username, token, created_date) 
values
    (@USERNAME, @TOKEN, @CREATED_DATE)
";

                NpgsqlCommand c = command as NpgsqlCommand;

                //c.Parameters.Add("UserId", NpgsqlDbType.Integer);
                c.Parameters.Add("username", NpgsqlDbType.Varchar, 50);

                c.Parameters.Add("token", NpgsqlDbType.Varchar, 100);
                c.Parameters.Add("created_date", NpgsqlDbType.Timestamp);

                c.Prepare();

                //c.Parameters["UserId"].Value = 1;
                c.Parameters["username"].Value = user.Username;
                c.Parameters["token"].Value = user.Username + "-mtcgToken";
                c.Parameters["created_date"].Value = DateTime.Now;

                command.ExecuteNonQuery();

                return user.Username + "-mtcgToken";
            }
        }

        public bool VerifyPassword(User user)
        {
            Console.WriteLine("Verifying password");
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                /*IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=mtcgdb");
            connection.Open();
            Console.WriteLine($"Connection open");*/
                {
                    IDbCommand command = connection.CreateCommand();
                    command.CommandText = @"
select password
from users
where username=@USERNAME
";
                    /*command.CommandText = @"
    select 
    case 
        when password=@PASSWORD
        then cast(1 as BIT)
        else cast(0 as bit)
    end as SamePassword
    from users
    where username=@USERNAME
    ";*/

                    NpgsqlCommand c = command as NpgsqlCommand;

                    //c.Parameters.Add("UserId", NpgsqlDbType.Integer);
                    c.Parameters.Add("username", NpgsqlDbType.Varchar, 50);
                    //c.Parameters.Add("password", NpgsqlDbType.Varchar, 100);

                    c.Prepare();

                    //c.Parameters["UserId"].Value = 1;
                    c.Parameters["username"].Value = user.Username;
                    //c.Parameters["password"].Value = user.Password;

                    Console.WriteLine("executing command");

                    var pw = command.ExecuteScalar();

                    Console.WriteLine($"{user.Password}");
                    Console.WriteLine($"{pw.ToString()}");
                    Console.WriteLine($"{pw}");

                    if (pw.ToString() == user.Password)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public string CheckAuthorization(string mtcgAuth)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                if (mtcgAuth == null || mtcgAuth == "")
                {
                    throw new Exception("401: Access token is missing or invalid");
                }
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
select username
from sessions
where token=@TOKEN
";
                //TODO: check for valid date
                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("token", NpgsqlDbType.Varchar, 50);
                c.Prepare();
                c.Parameters["token"].Value = mtcgAuth;

                var user = command.ExecuteScalar();
                return user.ToString();
            }
        }

        public List<string> CardIsDuplicate(List<Card> cards)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
select cardid
from cards
where cardid=@id
";
                NpgsqlCommand c = command as NpgsqlCommand;
                c.Parameters.Add("id", NpgsqlDbType.Varchar, 100);
                c.Prepare();

                List<string> duplicateCards = new List<string>();
                foreach (Card card in cards)
                {
                    c.Parameters["id"].Value = card.Id;
                    var duplicateId = command.ExecuteScalar();
                    if (duplicateId != null)
                    {
                        duplicateCards.Add(duplicateId.ToString());
                    }
                }
                return duplicateCards;
            }
        }

        public void CreateCards(List<Card> cards, int PackageId)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                Console.WriteLine("creating cards");
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
insert into cards
    (cardid, ownertype, packageid, name, damage, status)
values
    (@cardid, @ownertype, @packageid, @name, @damage, @status)
";
                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("cardid", NpgsqlDbType.Varchar, 255);
                c.Parameters.Add("ownertype", NpgsqlDbType.Integer);
                c.Parameters.Add("packageid", NpgsqlDbType.Integer);
                c.Parameters.Add("name", NpgsqlDbType.Varchar, 255);
                c.Parameters.Add("damage", NpgsqlDbType.Integer);
                c.Parameters.Add("status", NpgsqlDbType.Integer);
                c.Prepare();

                foreach (Card card in cards)
                {
                    c.Parameters["cardid"].Value = card.Id;
                    c.Parameters["ownertype"].Value = (int)CardOwnerType.package;
                    c.Parameters["packageid"].Value = PackageId;
                    c.Parameters["name"].Value = card.Name;
                    c.Parameters["damage"].Value = card.Damage;
                    c.Parameters["status"].Value = (int)CardStatus.package;

                    command.ExecuteNonQuery();
                }
                Console.WriteLine("finished creating cards");
            }
        }
        public void CreatePackage(List<Card> package)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                Console.WriteLine("creating package");
                if (CardIsDuplicate(package).Count != 0)
                {
                    Console.WriteLine("duplicate cards!");
                    throw new Exception("409: At least one card in the packages already exists");
                }

                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
insert into packages 
    (price, card1id, card2id, card3id, card4id, card5id)
values
    (@price, @card1id, @card2id, @card3id, @card4id, @card5id)
RETURNING packageid
";
                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("price", NpgsqlDbType.Integer);
                c.Parameters.Add("card1id", NpgsqlDbType.Varchar, 255);
                c.Parameters.Add("card2id", NpgsqlDbType.Varchar, 255);
                c.Parameters.Add("card3id", NpgsqlDbType.Varchar, 255);
                c.Parameters.Add("card4id", NpgsqlDbType.Varchar, 255);
                c.Parameters.Add("card5id", NpgsqlDbType.Varchar, 255);
                c.Prepare();
                Console.WriteLine("prepared parameters");
                c.Parameters["price"].Value = 5;
                c.Parameters["card1id"].Value = package[0].Id;
                c.Parameters["card2id"].Value = package[1].Id;
                c.Parameters["card3id"].Value = package[2].Id;
                c.Parameters["card4id"].Value = package[3].Id;
                c.Parameters["card5id"].Value = package[4].Id;
                Console.WriteLine("done preparing");
                int packageId = (int)command.ExecuteScalar();

                Console.WriteLine("package created!");
                CreateCards(package, packageId);
                Console.WriteLine("cards created!");
            }
        }

        public void AcquirePackage(string username)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                //search for first package in db an return all
                Console.WriteLine("trying to get package");
                var package = GetPackage();
                // Console.WriteLine(package);

                //get user coins from db and chack against package price
                var availableCoins = GetUserCoins(username);
                if (availableCoins == null || availableCoins < package.Price)
                {
                    throw new Exception("403: Not enough money for buying a card package");
                }
                Console.WriteLine($"{availableCoins} >= {package.Price}");

                //change all included 5 cards (ownertype, userid, packageid, status)
                AcquireCards(username, package.Cards); //(user, username, "", stack)
                                                       //remove user coins
                UpdateUserCoins(username, (availableCoins - package.Price));
                //delete package
                DeletePackage((int)package.Id);
                //Console.WriteLine("All done");
            }
        }

        private Package GetPackage()
        {
            //get first available package
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
select * from packages LIMIT 1";

                //NpgsqlCommand c = command as NpgsqlCommand;
                //c.Prepare();

                IDataReader reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    throw new Exception("404: No card package available for buying");
                }
                int packageId = reader.GetInt32(0);
                int price = reader.GetInt32(1);
                List<string> cardIds = new List<string>();
                cardIds.Add(reader.GetString(2));
                cardIds.Add(reader.GetString(3));
                cardIds.Add(reader.GetString(4));
                cardIds.Add(reader.GetString(5));
                cardIds.Add(reader.GetString(6));

                var package = new Package(packageId, price, cardIds);
                reader.Close();

                Console.WriteLine(package);
                return package;
            }
        }

        private int GetUserCoins(string username)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
select coins from users 
where username=@username";

                NpgsqlCommand c = command as NpgsqlCommand;
                c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
                c.Prepare();
                c.Parameters["username"].Value = username;
                //c.Prepare();

                int coins = (int)command.ExecuteScalar();
                return coins;
            }
        }

        public void AcquireCards(string username, List<Card> packageCards) {
            //change all included 5 cards (ownertype, userid, packageid, status)
            //(user, username, "", stack)
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
update cards
set ownertype=@ownertype, username=@username, status=@status
where cardid=@cardid";

                NpgsqlCommand c = command as NpgsqlCommand;
                c.Parameters.Add("ownertype", NpgsqlDbType.Integer);
                c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
                //c.Parameters.Add("packageid", NpgsqlDbType.Integer);
                c.Parameters.Add("status", NpgsqlDbType.Integer);
                c.Parameters.Add("cardid", NpgsqlDbType.Varchar, 255);
                c.Prepare();

                c.Parameters["username"].Value = username;
                c.Parameters["ownertype"].Value = (int)CardOwnerType.user;
                //c.Parameters["packageid"].Value = NULL;
                c.Parameters["status"].Value = (int)CardStatus.stack;

                foreach (var card in packageCards)
                {
                    c.Parameters["cardid"].Value = card.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        private void UpdateUserCoins(string username, int coins)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
update users
set coins=@coins
where username=@username";

                NpgsqlCommand c = command as NpgsqlCommand;
                c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
                c.Parameters.Add("coins", NpgsqlDbType.Integer);
                c.Prepare();
                c.Parameters["username"].Value = username;
                c.Parameters["coins"].Value = coins;

                command.ExecuteNonQuery();
            }
        }

        private void DeletePackage(int packageid)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
delete from packages
where packageid=@packageid";

                NpgsqlCommand c = command as NpgsqlCommand;
                c.Parameters.Add("packageid", NpgsqlDbType.Integer);
                c.Prepare();
                c.Parameters["packageid"].Value = packageid;

                command.ExecuteNonQuery();
            }
        }

        public List<Card> GetStack(string username)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
select * from cards
where username=@username and ownertype=@ownertype";

                NpgsqlCommand c = command as NpgsqlCommand;
                c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
                c.Parameters.Add("ownertype", NpgsqlDbType.Integer);
                c.Prepare();
                c.Parameters["username"].Value = username;
                c.Parameters["ownertype"].Value = (int)CardOwnerType.user;

                List<Card> stack = new List<Card>();
                IDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Card card = new Card(
                        reader.GetString(0),
                        reader.GetString(4),
                        reader.GetInt32(5),
                        reader.GetString(2),
                        (CardStatus)reader.GetInt32(6)
                    );
                    stack.Add(card);
                    //Console.WriteLine($"{reader[0]}");

                }
                return stack;
            }
        }

        public List<Card> GetDeck(string username)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
select * from cards
where username=@username and ownertype=@ownertype and status=@status";

                NpgsqlCommand c = command as NpgsqlCommand;
                c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
                c.Parameters.Add("ownertype", NpgsqlDbType.Integer);
                c.Parameters.Add("status", NpgsqlDbType.Integer);
                c.Prepare();
                c.Parameters["username"].Value = username;
                c.Parameters["ownertype"].Value = (int)CardOwnerType.user;
                c.Parameters["status"].Value = (int)CardStatus.deck;

                List<Card> deck = new List<Card>();
                IDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Card card = new Card(
                        reader.GetString(0),
                        reader.GetString(4),
                        reader.GetInt32(5),
                        reader.GetString(2),
                        (CardStatus)reader.GetInt32(6)
                    );
                    deck.Add(card);
                    //Console.WriteLine($"{reader[0]}");

                }
                return deck;
            }
        }

        public void ConfigureDeck(string username, List<string> cardids)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                if (!CardsAreInStackOrDeck(username, cardids))
                {
                    throw new Exception("403: At least one of the provided cards does not belong to the user or is not available.");
                }

                //remove old cards from deck
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
update cards
set status=@statusstack
where username=@username and ownertype=@ownertype and status=@statusdeck";

                NpgsqlCommand c = command as NpgsqlCommand;
                c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
                c.Parameters.Add("ownertype", NpgsqlDbType.Integer);
                c.Parameters.Add("statusdeck", NpgsqlDbType.Integer);
                c.Parameters.Add("statusstack", NpgsqlDbType.Integer);
                c.Prepare();
                c.Parameters["username"].Value = username;
                c.Parameters["ownertype"].Value = (int)CardOwnerType.user;
                c.Parameters["statusdeck"].Value = (int)CardStatus.deck;
                c.Parameters["statusstack"].Value = (int)CardStatus.stack;

                command.ExecuteNonQuery();
                Console.WriteLine($"removed");

                //add new cards to deck
                command = connection.CreateCommand();
                command.CommandText = @"
update cards
set status=@statusdeck
where username=@username AND ownertype=@ownertype and cardid=@cardid"; //username/type redundant due to !CardsAreInStackOrDeck()

                c = command as NpgsqlCommand;
                c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
                c.Parameters.Add("ownertype", NpgsqlDbType.Integer);
                c.Parameters.Add("statusdeck", NpgsqlDbType.Integer);
                c.Parameters.Add("statusstack", NpgsqlDbType.Integer);
                c.Parameters.Add("cardid", NpgsqlDbType.Varchar, 255);
                c.Prepare();
                c.Parameters["username"].Value = username;
                c.Parameters["ownertype"].Value = (int)CardOwnerType.user;
                c.Parameters["statusdeck"].Value = (int)CardStatus.deck;
                c.Parameters["statusstack"].Value = (int)CardStatus.stack;

                foreach (string id in cardids)
                {
                    c.Parameters["cardid"].Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool CardsAreInStackOrDeck(string username, List<string> cardids)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
select cardid from cards
where cardid=@cardid
and username=@username and ownertype=@ownertype 
and (status=@statusdeck or status=@statusstack)";

                NpgsqlCommand c = command as NpgsqlCommand;
                c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
                c.Parameters.Add("ownertype", NpgsqlDbType.Integer);
                c.Parameters.Add("statusdeck", NpgsqlDbType.Integer);
                c.Parameters.Add("statusstack", NpgsqlDbType.Integer);
                c.Parameters.Add("cardid", NpgsqlDbType.Varchar, 255);
                c.Prepare();
                c.Parameters["username"].Value = username;
                c.Parameters["ownertype"].Value = (int)CardOwnerType.user;
                c.Parameters["statusdeck"].Value = (int)CardStatus.deck;
                c.Parameters["statusstack"].Value = (int)CardStatus.stack;

                foreach (string id in cardids)
                {
                    c.Parameters["cardid"].Value = id;

                    if (command.ExecuteScalar() == null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public void RemoveDeck(string username)
        {
            //adds deck cards back to stack
            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
update cards
set status=@statusstack
where username=@username and ownertype=@ownertype and status=@statusdeck";

                NpgsqlCommand c = command as NpgsqlCommand;
                c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
                c.Parameters.Add("ownertype", NpgsqlDbType.Integer);
                c.Parameters.Add("statusstack", NpgsqlDbType.Integer);
                c.Parameters.Add("statusdeck", NpgsqlDbType.Integer);
                c.Prepare();
                c.Parameters["username"].Value = username;
                c.Parameters["ownertype"].Value = (int)CardOwnerType.user;
                c.Parameters["statusstack"].Value = (int)CardStatus.stack;
                c.Parameters["statusdeck"].Value = (int)CardStatus.deck;

                command.ExecuteNonQuery();
            }
        }
    }
}







/*
private static void WriteUserToDB(IList<UserRecord> data)
{
    IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=simpledatastore");
    connection.Open();
    Console.WriteLine($"Connection open");
    {

        IDbCommand command = connection.CreateCommand();
        command.CommandText = "delete from users";
        command.ExecuteNonQuery();
    }
    {
        IDbCommand command = connection.CreateCommand();
        command.CommandText = @"
insert into users 
(userid, username, password, coins) 
values
(@USERID, @USERNAME, @PASSWORD, @COINS)
";

        var pFID = command.CreateParameter();
        pFID.DbType = DbType.String;
        pFID.ParameterName = "fid";
        pFID.Size = 50;
        command.Parameters.Add(pFID);

        NpgsqlCommand c = command as NpgsqlCommand;

        c.Parameters.Add("userid", NpgsqlDbType.Integer);
        c.Parameters.Add("username", NpgsqlDbType.Varchar, 50);

        c.Parameters.Add("password", NpgsqlDbType.Varchar, 50);
        c.Parameters.Add("coins", NpgsqlDbType.Integer);

        c.Prepare();

        foreach (UserRecord item in data)
        {
            //command.Parameters["fid"].Value = item.FID;
            c.Parameters["userid"].Value = item.USERID;
            c.Parameters["username"].Value = item.USERNAME;
            c.Parameters["password"].Value = item.PASSWORD;
            c.Parameters["coins"].Value = item.COINS;

            command.ExecuteNonQuery();
        }
    }
}*/

/*public void CreateUser(UserRecord player)
{
    //deprecated
    //save User to DB
    Console.WriteLine("Hello User");
    Console.WriteLine($"Created User {player.ToString()}");
    var user = new List<UserRecord>
    {
        new UserRecord(
        (new Random().Next()),
        player.USERNAME,
        player.PASSWORD,
        20
    )
    };
    Console.WriteLine($"About to add User {user[0].USERNAME} to db");
    WriteUserToDB(user);
}*/
/*
public record UserRecord(
    int? USERID,
    string USERNAME,
    string PASSWORD,
    int? COINS
);

public class UserClass
{
    public int? USERID { get; set; }
    public string USERNAME { get; set; }
    public string PASSWORD { get; set; }
    public int? COINS { get; set; }

    public UserClass From(UserRecord user)
    {
        this.USERID = user.USERID;
        this.USERNAME = user.USERNAME;
        this.PASSWORD = user.PASSWORD;
        this.COINS = user.COINS;
        return this;
    }
}*/