//using MonsterTradingCardsGame.Model;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace MonsterTradingCardsGame.DAL
{
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
    }

    public class PostgreSQLRepository
    {
        public void CreateUser(UserRecord player)
        {
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
        }

        private static void WriteUserToDB(IList<UserRecord> data)
        {
            IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=simpledatastore");
            connection.Open();
            Console.WriteLine($"Connection open");
            {
                /*
                IDbCommand command = connection.CreateCommand();
                command.CommandText = "delete from users";
                command.ExecuteNonQuery();*/
            }
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"
insert into users 
    (userid, username, password, coins) 
values
    (@USERID, @USERNAME, @PASSWORD, @COINS)
";
                /*
                var pFID = command.CreateParameter();
                pFID.DbType = DbType.String;
                pFID.ParameterName = "fid";
                pFID.Size = 50;
                command.Parameters.Add(pFID);*/

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
        }
    }
}