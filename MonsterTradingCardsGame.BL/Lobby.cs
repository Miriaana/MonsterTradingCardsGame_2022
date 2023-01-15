using MTCGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MTCGame.BL
{
    public enum BattleState { Waiting, InProgess, Finished, Closed }
    public class Lobby
    {
        public static List<User> Queue = new List<User>();

        //static List<[string, BattleState]> BattlesFinished = new List<string>();
        public static Dictionary<string, BattleState> UserBattleState = new Dictionary<string, BattleState>();

        public static List<BattleLog> Logs = new List<BattleLog>();

        public Lobby() {
            Console.WriteLine("running lobby ctor");
        }

        public BattleLog Join(User player)
        {
            UserBattleState[player.Username]=BattleState.Waiting;
            if (Queue.Count > 0)
            {
                User player2 = Queue.First();
                Queue.RemoveAt(0);
                Console.WriteLine($"getting player from queue and starting battle");
                Task.Factory.StartNew(() =>
                {
                    new GameHandler().Battle(player, player2);
                });
            }
            else
            {
                Queue.Add(player);
                Console.WriteLine($"adding player to queue");
            }

            Console.WriteLine($"{player.Username}: waiting for battle to be done");
            while (!(UserBattleState[player.Username] == BattleState.Finished))
            {
                //Console.WriteLine($"{player.Username}: ...");
                Thread.Sleep(25);
            }

            Console.WriteLine($"{player.Username}: battle finished");

            BattleLog battleLog = new BattleLog();
            foreach(BattleLog log in Logs) { 
                if(log.Player1.Username == player.Username || log.Player2.Username == player.Username) {
                    Console.WriteLine($"{player.Username}: found log");
                    battleLog = log;
                    break;
                }
            }
            UserBattleState[player.Username] = BattleState.Closed;

            if(battleLog.Log == "")
            {
                throw new Exception("500: No BattleLog found");
            }
            return battleLog;
        }
    }
}
