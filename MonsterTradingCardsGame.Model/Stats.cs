using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Model
{
    public class Stats
    {
        public int elo { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public Stats() { 
            elo = 0;
            wins = 0;
            losses = 0;
        }

    }
}
