//using MonsterTradingCardsGame.Model;
using MTCGame.Model;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace MTCGame.DAL
{
    public class PostgreSQLRepository
    {
        IDbConnection connection;
        public PostgreSQLRepository() {
            connection = new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=mtcgdb");
            connection.Open();
            Console.WriteLine($"Connection open");
        }
        public void CreateUser(User user)
        {
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
            /*IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=mtcgdb");
            connection.Open();
            Console.WriteLine($"Connection open");*/
            {
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
            Console.WriteLine("Verifying password");/*
            IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=mtcgdb");
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
                /*
                IDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader[0]}");
                }
                Console.WriteLine("end");*/
                //return false;
            }
        }

        public string CheckAuthorization(string mtcgAuth)
        {
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

        public List<string> CardIsDuplicate(List<Card> cards)
        {
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

        public void CreateCards(List<Card> cards, int PackageId)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = @"
insert into cards
    (cardid, ownertype, packageid, name, damage, status)
values
    (@cardid, @ownertype, @packageid, @name, @damage, @status)
";
            NpgsqlCommand c = command as NpgsqlCommand;

            c.Parameters.Add("cardid", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("ownertype", NpgsqlDbType.Varchar, 50);
            c.Parameters.Add("packageid", NpgsqlDbType.Integer);
            c.Parameters.Add("name", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("damage", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("status", NpgsqlDbType.Varchar, 50);
            c.Prepare();

            foreach(Card card in cards)
            {
                c.Parameters["cardid"].Value = card.Id;
                c.Parameters["ownertype"].Value = "Package";
                c.Parameters["packageid"].Value = PackageId;
                c.Parameters["name"].Value = card.Name;
                c.Parameters["damage"].Value = card.Damage;
                c.Parameters["status"].Value = "Package";

                command.ExecuteNonQuery();
            }
        }
        public void CreatePackage(List<Card> package)
        {   
            if(CardIsDuplicate(package).Count != 0)
            {
                Console.WriteLine("duplicate cards!");
                throw new Exception("409: At least one card in the packages already exists");
            }

            IDbCommand command = connection.CreateCommand();
            command.CommandText = @"
insert into packages 
    (price, card1id, card2id, card3id, card4id, card5id)
values
    (@price, @card1id, @card2id, @card3id, @card4id, @card5id);
SELECT SCOPE_IDENTITY()
";
            //alt: OUTPUT Inserted.ID btweeninsandval
            NpgsqlCommand c = command as NpgsqlCommand;

            c.Parameters.Add("price", NpgsqlDbType.Integer);
            c.Parameters.Add("card1id", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("card2id", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("card3id", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("card4id", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("card5id", NpgsqlDbType.Varchar, 255);
            c.Prepare();
            c.Parameters["price"].Value = 20;
            c.Parameters["card1id"].Value = package[0].Id;
            c.Parameters["card2id"].Value = package[1].Id;
            c.Parameters["card3id"].Value = package[2].Id;
            c.Parameters["card4id"].Value = package[3].Id;
            c.Parameters["card5id"].Value = package[4].Id;

            int packageId = (int)command.ExecuteScalar();

            CreateCards(package, packageId);
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