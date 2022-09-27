using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class MonsterCard : Card
    {
        public MonsterCard(string name, int damage, Element element) : base(name, damage, element)
        {
            _type = CardType.monster;
        }
    }
}
