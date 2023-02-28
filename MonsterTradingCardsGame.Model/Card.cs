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
    public enum CardElement { regular, fire, water };
    public enum MajorCardType { Spell, Monster };
    public enum MinorCardType { Spell, Goblin, Dragon, Wizzard, Ork, Knight, Kraken, Elf, Troll };
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

        public float? BattleDamage { get; set; }
        public string? BattleText { get; set; }

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
            string[] nameParts = Regex.Split(Name, @"(?<!^)(?=[A-Z])");
            string typePart;
            if(nameParts.Length == 1)
            {
                typePart = nameParts[0];
            }
            else if(nameParts.Length == 2)
            {
                typePart = nameParts[1];
                //fill Element
                if (Enum.TryParse(nameParts[0], true, out CardElement element))
                {
                    if (Enum.IsDefined(typeof(CardElement), element))
                    {
                        Element = element;
                    }
                    else
                    {
                        throw new Exception($"500: Conversion of Element not possible(1)");
                    }
                }
                else
                {
                    throw new Exception($"500: Conversion of Element not possible(2)");
                }
            }
            else
            {
                throw new Exception($"500: Unable to split {Name}");
            }

            

            //fill card types
            if (Enum.TryParse(typePart, true, out MinorCardType minorType))
            {
                if (Enum.IsDefined(typeof(MinorCardType), minorType))
                {
                    MinorType = minorType;
                }
                else
                {
                    throw new Exception($"500: Conversion of Element not possible(1)");
                }
            }
            else
            {
                throw new Exception($"500: Conversion of Element not possible(2)");
            }

            if (MinorType == MinorCardType.Spell)
            {
                MajorType = MajorCardType.Spell;
            }
            else
            {
                MajorType = MajorCardType.Monster;
            }
            
            //init 
            BattleDamage = Damage;
            BattleText = string.Empty;
        }

        public void Attack(Card enemy)
        {
            //set base damage
            BattleDamage = (int)Damage;
            BattleText = string.Empty;

            //pure Monsterfight -> Element (usually) not relevant
            if (MajorType==MajorCardType.Monster && enemy.MajorType == MajorCardType.Monster)
            {
                //Exceptions (monster- or elemental weaknesses)
                if(MinorType == MinorCardType.Goblin && enemy.MinorType == MinorCardType.Dragon)
                {
                    BattleDamage = 0.0f;
                }
                else if (MinorType == MinorCardType.Ork && enemy.MinorType == MinorCardType.Wizzard)
                {
                    BattleDamage = 0.0f;
                }
                else if (MinorType == MinorCardType.Dragon && enemy.MinorType == MinorCardType.Elf && enemy.Element == CardElement.fire)
                {
                    BattleDamage = 0.0f;
                }
                //ordinary Monsterfight
                else
                {
                    //keeps ordinary battle damage
                }
        
            }

            //pure Spellfight or mixed fight -> Element relevant
            else
            {
                //Exceptions (monster- or elemental weaknesses)
                if (MinorType == MinorCardType.Knight && enemy.Element == CardElement.water)
                {
                    BattleDamage = 0.0f;
                }
                else if (enemy.MinorType == MinorCardType.Kraken)
                {
                    BattleDamage = 0.0f;
                }
                //ordinary Spell-/mixed fight
                else
                {
                    
                    if(1 == IsEffectiveAgainst(enemy.Element))
                        //this cards element is strong/effective
                    {
                        BattleDamage *= 2;
                    }
                    else if (-1 == IsEffectiveAgainst(enemy.Element))
                        //this cards element is weak/not effective
                    {
                        BattleDamage /= 2;
                    }
                    //else no effect (cards have same element)
                }
            }
        }

        public int IsEffectiveAgainst(CardElement enemyElement)
        {
            if(Element == CardElement.regular)
            {
                if (enemyElement == CardElement.water)
                    return 1;
                else if (enemyElement == CardElement.fire)
                    return -1;
            }
            else if (Element == CardElement.water)
            {
                if (enemyElement == CardElement.fire)
                    return 1;
                else if (enemyElement == CardElement.regular)
                    return -1;
            }
            else if (Element == CardElement.fire)
            {
                if (enemyElement == CardElement.regular)
                    return 1;
                else if (enemyElement == CardElement.water)
                    return -1;
            }

            //else Elements are the same
            return 0;
        }
    }
}
