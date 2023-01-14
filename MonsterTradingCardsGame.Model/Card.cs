using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Model
{
    public class Card
    {
        public Card() { }
        public string? Id { get; set; }
        //public enum Element { normal, fire, water};   //what accessability?
        //public enum CardType { spell, monster };

        public string? Name { get; set; }
        public float? Damage { get; set; }

        //protected string _name;
        //protected CardType _type;
        //protected Element _element;
        //protected readonly int _damage;

        
        public Card(string cardId, string name, int damage)
        {
            Id = cardId;
            Name = name;
            Damage = damage;
            //_type = type;         //what do with this?! how show you have to implement this in child ctor?
        }
    }
}
