using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MTCGame.Model
{
    public enum CardElement { normal, fire, water };
    public enum MajorCardType { spell, monster };
    public enum MinorCardType { spell, monster };
    public enum CardOwnerType { package, user };
    public enum CardStatus { package, stack, deck, trading }
    public class Card
    {
        public string? Username { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public float? Damage { get; set; }
        public CardStatus? Status { get; set; }
        public MajorCardType? MajorType { get; set; }
        public MinorCardType? MinorType { get; set; }
        public CardElement Element { get; set; }

        //protected string _name;
        //protected CardType _type;
        //protected Element _element;
        //protected readonly int _damage;

        public Card() { }
        public Card(string cardId)
        {
            Id = cardId;
        }
        public Card(string cardId, string name, int damage)
        {
            Id = cardId;
            Name = name;
            Damage = damage;
            //_type = type;         //what do with this?! how show you have to implement this in child ctor?
        }

        public Card(string cardId, string name, int damage, string username, CardStatus status)
        {
            Id = cardId;
            Name = name;
            Damage = damage;
            Username = username;
            Status = status;
            //_type = type;         //what do with this?! how show you have to implement this in child ctor?
        }

        public void FillTypes()
        {
            /*
            string[] nameParts = Regex.Split(Name, @"(?<!^)(?=[A-Z])");
            if (CardElement.TryParse(nameParts[1], out CardElement _))
            {

            }
            else
            {
                throw new Exception("500: couldn't parse card element");
            }*/
        }
    }
}
