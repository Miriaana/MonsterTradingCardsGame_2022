using MTCGame.DAL;
using MTCGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static MTCGame.BL.BattleLog;

namespace MTCGame.BL
{
    public class StatHandler
    {
        public Stats GetStats(string mtcgAuth)
        {
            var repo = new PostgreSQLRepository();

            string auth = repo.CheckAuthorization(mtcgAuth);
            if (string.IsNullOrEmpty(auth))
            {
                throw new Exception("401: Access token is missing or invalid");
            }
            Stats stats = repo.GetStats(auth);
            return stats;
        }

        public void UpdateStats(string username, BattleOutcome outcome)
        {
            if (outcome == BattleOutcome.draw)
            {
                Console.WriteLine($"UpdateStats not changing anything because it was a draw");
                //no need to update anything if there was no winner
                return;
            }

            var repo = new PostgreSQLRepository();
            Stats stats = GetStats(username);

            if (outcome == BattleOutcome.win) {
                stats.elo += 3;
                stats.wins++;
            }
            else //if BattleOutcome is loss
            {
                stats.elo -= 5;
                stats.losses++;
            }

            repo.UpdateStats(username, stats);
        }
    }
}
