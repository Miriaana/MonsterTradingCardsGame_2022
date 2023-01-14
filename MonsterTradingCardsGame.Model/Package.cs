using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Model
{
    public class Package
    {
        public int? Id { get; set; }
        public int Price { get; set; }
        public List<Card> Cards { get; set; }

        public Package() { }
        public Package(int id, int price, List<Card> cards)
        {
            Id = id;
            Price = price;
            Cards = cards;
        }
        public Package(int id, int price, List<string> cardIds)
        {
            Id = id;
            Price = price;
            Cards = new List<Card>();
            foreach(string cardId in cardIds)
            {
                Cards.Add(new Card(cardId));
            }
        }
    }
}
