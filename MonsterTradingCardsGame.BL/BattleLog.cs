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
        public User Player1;

        public User Player2;

        public string Log;

        public BattleLog() { }
        public BattleLog(User Player1, User Player2)
        {
            this.Player1 = Player1;
            this.Player2 = Player2;
            Log = "";
        }
    }
}
