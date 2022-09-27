using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class SpellCard : Card
    {
        public SpellCard(string name, int damage, Element element) : base(name, damage, element)
        {
            _type = CardType.spell;
        }
    }
}
