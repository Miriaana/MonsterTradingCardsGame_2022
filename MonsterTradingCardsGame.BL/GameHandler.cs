using MTCGame.Model;
using System.Numerics;

namespace MTCGame.BL 
{
    public class GameHandler
    {
        Lobby Lobby;
        BattleLog Log1;
        BattleLog Log2;
        public GameHandler() { 
            Lobby= new Lobby();
            Log1 = new BattleLog();
            Log2 = new BattleLog();
        }
        public void Battle(User Player1, User Player2)
        {
            //set state to waiting and setup
            Lobby.UserBattleState[Player1.Username] = BattleState.InProgess;
            Lobby.UserBattleState[Player2.Username] = BattleState.InProgess;

            Log1.Player = Player1;
            Log1.Enemy = Player2;
            Log2.Player = Player2;
            Log2.Enemy = Player1;
            //-----------------------------------------
            //play game
            Console.WriteLine("Run Game");
            Thread.Sleep(1000);

            int battleRound = 0;
            while (Player1.Deck.Count != 0 
                && Player2.Deck.Count != 0
                && battleRound < 100) 
            {
                battleRound++;
                Card card1 = Player1.Deck.First();
                Card card2 = Player2.Deck.First();
                //player 1 attack
                card1.Attack(card2);
                if(card1.BattleText!= null && card1.BattleText != string.Empty)
                {
                    Log1.LogLine(card1.BattleText);
                }
                //player 2 attack
                card2.Attack(card1);
                if (card2.BattleText != null && card2.BattleText != string.Empty)
                {
                    Log2.LogLine(card2.BattleText);
                }
                //evaluate round
                if (card1.BattleDamage > card2.BattleDamage) //player 1 wins round
                {
                    Log1.LogLine($"WIN:  Your {card1.Name} defeated the enemy's {card2.Name} with {card1.BattleDamage} : {card2.BattleDamage} DamagePoints");
                    Log2.LogLine($"LOSS: The enemy's {card1.Name} defeated your {card2.Name} with {card1.BattleDamage} : {card2.BattleDamage} DamagePoints");
                    Player1.Deck.Add(card2);
                    Player2.Deck.RemoveAt(0);
                }
                else if(card2.BattleDamage > card1.BattleDamage) //player 2 wins round
                {
                    Log2.LogLine($"WIN:  Your {card2.Name} defeated the enemy's {card1.Name} with {card2.BattleDamage} : {card1.BattleDamage} DamagePoints");
                    Log1.LogLine($"LOSS: The enemy's {card2.Name} defeated your {card1.Name} with {card2.BattleDamage} : {card1.BattleDamage} DamagePoints");
                    Player2.Deck.Add(card1);
                    Player1.Deck.RemoveAt(0);
                }
                else //draw
                {
                    Log1.LogLine($"DRAW: Your {card1.Name} with {card1.BattleDamage} DamagePoints was just as strong the enemy's {card2.Name}");
                    Log2.LogLine($"DRAW: Your {card2.Name} with {card2.BattleDamage} DamagePoints was just as strong the enemy's {card1.Name}");
                    Player1.Deck.Add(card1);
                    Player1.Deck.RemoveAt(0);
                    Player2.Deck.Add(card2);
                    Player2.Deck.RemoveAt(0);
                }
            }
            Console.WriteLine("Battle Finished");
            Thread.Sleep(1000);

            //evaluate winner
            if(Player1.Deck.Count == 0) //Player 1 lost
            {
                Log1.LogLine($"\n\tYou lost.\n");
                Log2.LogLine($"\n\tYOU WON!\n");
            } 
            else if (Player2.Deck.Count == 0)
            {
                Log2.LogLine($"\n\tYou lost.\n");
                Log1.LogLine($"\n\tYOU WON!\n");
            } 
            else
            {
                Log1.LogLine($"\n\tDraw!\n");
                Log2.LogLine($"\n\tDraw!\n");
            }


            //---------------------------------------
            //game finished
            //todo: lock lobby

            Log1.Player.Deck = Player1.Deck;
            Log1.Enemy.Deck = Player2.Deck;
            Log2.Player.Deck = Player2.Deck;
            Log2.Enemy.Deck = Player1.Deck;

            Lobby.Logs[Log1.Player.Username] = Log1;
            Lobby.Logs[Log2.Player.Username] = Log2;

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