using MTCGame.Model;
using System;
using System.Collections.Concurrent;
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
        public static readonly object QueueLock = new object();
        public static ConcurrentQueue<User> Queue = new ConcurrentQueue<User>();

        //static List<[string, BattleState]> BattlesFinished = new List<string>();

        public static ConcurrentDictionary<string, BattleState> UserBattleState = new ConcurrentDictionary<string, BattleState>();

        //public static readonly object LogsLock = new object();
        public static ConcurrentDictionary<string, BattleLog> Logs = new ConcurrentDictionary<string, BattleLog>();

        public Lobby() {
            Console.WriteLine("running lobby ctor");
        }

        public BattleLog Join(User player)
        {
            UserBattleState[player.Username]=BattleState.Waiting;

            lock(QueueLock)
            {
                if (Queue.IsEmpty)
                {
                    Queue.Enqueue(player);

                    Console.Write($"added player to queue");
                }
                else
                {
                    Console.WriteLine($"getting player from queue and starting battle");
                    if (!Queue.TryDequeue(out User player2))
                    {
                        throw new Exception("500: wasn't able to dequeue player2");
                    }
                    Task.Factory.StartNew(() =>
                    {
                        new GameHandler().Battle(player, player2);
                    });
                }
            }

            Console.WriteLine($"{player.Username}: waiting for battle to be done");
            while (!(UserBattleState[player.Username] == BattleState.Finished))
            {
                Thread.Sleep(25);
            }

            Console.WriteLine($"{player.Username}: battle finished");

            //retrieve BattleLog

            BattleLog battleLog = Logs[player.Username];
            UserBattleState[player.Username] = BattleState.Closed;

            if(battleLog.LogString == string.Empty)
            {
                throw new Exception("500: No BattleLog found");
            }
            return battleLog;
        }
    }
}
