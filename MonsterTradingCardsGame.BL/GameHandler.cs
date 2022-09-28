using MonsterTradingCardsGame.Model;

namespace MonsterTradingCardsGame.BL
{
    public class GameHandler
    {
        public void PlayBattle(User player1, User player2)
        {
            //magic happens
            Console.WriteLine($"{player1.ToString()} wins!");
        }
    }
}