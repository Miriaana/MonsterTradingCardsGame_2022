using MTCGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.BL
{
    public class BattleLog
    {
        public User Player;

        public User Enemy;

        public string LogString;

        public BattleLog() { 
            LogString = string.Empty;
            Enemy = new User();
            Player= new User();
        }
        public BattleLog(User Player1, User Player2)
        {
            this.Player = Player1;
            this.Enemy = Player2;
            LogString = "";
        }
        public void LogLine(string line)
        {
            if (line != null /*&& line != string.Empty*/)
                LogString += line + "\n";
        }
    }
}
