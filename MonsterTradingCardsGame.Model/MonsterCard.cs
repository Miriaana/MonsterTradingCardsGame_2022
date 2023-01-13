using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MTCGame.Model
{
    public class MonsterCard : Card
    {
        public MonsterCard(string name, int damage, Element element) : base(name, damage, element)
        {
            _type = CardType.monster;
        }
    }
}
