//using MonsterTradingCardsGame.Model;
using MonsterTradingCardsGame.Model;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace MonsterTradingCardsGame.DAL
{
    public class PostgreSQLRepository
    {
        public void CreateUser(User user)
        {
            IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=mtcgdb");
            connection.Open();
            Console.WriteLine($"Connection open");
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
insert into users 
    (Username, Password, Coins, Elo) 
values
    (@USERNAME, @PASSWORD, @COINS, @ELO)
";
                Console.WriteLine($"Connection open 2");
                NpgsqlCommand c = command as NpgsqlCommand;

                //c.Parameters.Add("UserId", NpgsqlDbType.Integer);
                c.Parameters.Add("Username", NpgsqlDbType.Varchar, 50);

                c.Parameters.Add("Password", NpgsqlDbType.Varchar, 50);
                c.Parameters.Add("Coins", NpgsqlDbType.Integer);
                c.Parameters.Add("Elo", NpgsqlDbType.Integer);
                Console.WriteLine($"Connection open 3");
                c.Prepare();
                Console.WriteLine($"Connection open 4");
                //c.Parameters["UserId"].Value = 1;
                c.Parameters["Username"].Value = user.Username;
                c.Parameters["Password"].Value = user.Password;
                c.Parameters["Coins"].Value = 20;
                c.Parameters["Elo"].Value = 0;
                Console.WriteLine($"Connection open 5");
                command.ExecuteNonQuery();
                Console.WriteLine($"Connection open 6");
            }
        }

        public User GetUser(User user)
        {
            Console.WriteLine("getting user info");
            return user;
        }

        public void UpdateProfile(User user) 
        {
            Console.WriteLine("updating profile");
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