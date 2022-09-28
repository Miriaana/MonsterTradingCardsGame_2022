using MonsterTradingCardsGame.Model;
using System.Reflection.Metadata;

namespace MonsterTradingCardsGame.DAL
{
    public class PostgreSQLRepository
    {
        public void CreateUser(User player)
        {
            Console.WriteLine("Hello User");
            Console.WriteLine($"Created User {player.ToString()}");
        }
    }
}