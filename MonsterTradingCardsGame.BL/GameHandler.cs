using MTCGame.Model;
using System.Numerics;

namespace MTCGame.BL 
{
    public class GameHandler
    {
        Lobby Lobby;
        BattleLog BattleLog;
        public GameHandler() { 
            Lobby= new Lobby();
            BattleLog= new BattleLog();
        }
        public void Battle(User Player1, User Player2)
        {
            //set state to waiting and setup
            Lobby.UserBattleState[Player1.Username] = BattleState.InProgess;
            Lobby.UserBattleState[Player2.Username] = BattleState.InProgess;

            BattleLog.Player1 = Player1;
            BattleLog.Player2 = Player2;

            //play game
            Thread.Sleep(1000);
            Console.WriteLine("Battle ongoing");
            Thread.Sleep(5000);
            Console.WriteLine("Battle finished");

            //demo: player2 lost all his cards
            foreach(Card card in Player2.Deck)
            {
                BattleLog.Player1.Deck.Add(card);
            }
            BattleLog.Player2.Deck.Clear();

            //game finished
            Lobby.Logs.Add(BattleLog);

            Lobby.UserBattleState[Player1.Username] = BattleState.Finished;
            Lobby.UserBattleState[Player2.Username] = BattleState.Finished;
            Console.WriteLine("end Battle");
        }
        /*
        /// <summary>
        /// receives users with their decks, returns the winner; 
        /// returns 0 when no winner, 1 when 1st player wins, 2 when 2nd player wins,
        /// -1 on error or unaccounted for options
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        /// <returns></returns>
        public int PlayBattle(User player1, User player2)
        {
            //magic happens
            Console.WriteLine($"{player1.ToString()} wins!");
            return -1;
        }*/
    }
}