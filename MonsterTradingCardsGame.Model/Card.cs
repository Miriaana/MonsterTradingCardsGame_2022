using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Model
{
    public abstract class Card
    {
        public enum Element { normal, fire, water};   //what accessability?
        public enum CardType { spell, monster };

        protected string _name;
        protected CardType _type;
        protected Element _element;
        protected readonly int _damage;

        public Card(string name, int damage, Element element)
        {
            _name = name;
            _damage = damage;
            _element = element;
            //_type = type;         //what do with this?! how show you have to implement this in child ctor?
        }
    }
}
