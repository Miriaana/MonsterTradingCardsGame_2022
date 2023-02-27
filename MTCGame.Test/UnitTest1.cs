using MTCGame.Model;
//using Moq;
using MTCGame.BL;
using static System.Net.Mime.MediaTypeNames;

namespace MTCGame.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("Hello World")]
        public void BattleLog_LogLine_DoesNotThrowEvenIfStringEmptyOrNull(string line)
        {
            //arrange
            var logs = new BattleLog();

            //Act + Assert
            Assert.DoesNotThrow(() => logs.LogLine(line));
        }

        [TestCase(null, null, "")]
        [TestCase("", "", "")]
        [TestCase("Hello World!", "Welcome", "Hello World!\nWelcome\n")]
        public void BattleLog_LogLine_LogsToLogStringAsExpected(string line1, string line2, string expectedLog)
        {
            //arrange
            var logs = new BattleLog();

            //Act
            logs.LogLine(line1);
            logs.LogLine(line2);

            //Assert
            Assert.That(expectedLog, Is.EqualTo(logs.LogString));
        }

        
        [TestCase("Knight", 1.0f)]
        [TestCase("WaterGoblin", 14.0f)]
        [TestCase("WaterGoblin", 14)]
        [TestCase("Dragon", 60.0f)]
        [TestCase("FireSpell", 25.0f)]
        [TestCase("WaterWizzard", 25.0f)]
        [TestCase("Kraken", 25.0f)]
        [TestCase("FireElf", 25.0f)]
        [TestCase("Ork", 25.0f)]
        [TestCase("FireTroll", 25.0f)]
        public void Card_FillTypes_DoesntThrowException(string name, float damage)
        {
            //Arrange
            Card card = new Card();
            card.Name = name;
            card.Damage = damage;

            //Act

            //Assert
            Assert.DoesNotThrow(() => card.FillTypes());
        }

        [TestCase("", 1.0f)]
        [TestCase("Water", 25.0f)]
        [TestCase("DendroGoblin", 13.0f)]
        [TestCase("ElfFire", 10.0f)]
        [TestCase("Watergoblin", 10.0f)]
        public void Card_FillTypes_DoesThrowException(string name, float damage)
        {
            //Arrange
            Card card = new Card();
            card.Name = name;
            card.Damage = damage;

            //Act / Assert

            Assert.Throws<Exception>(() => card.FillTypes());
        }

        //{ Spell, Goblin, Dragon, Wizzard, Ork, Knight, Kraken, Elf, Troll }
        //Monster Fights
        [TestCase("Goblin", 10.0f, 10.0f, "Wizzard")]
        [TestCase("FireOrk", 10.0f, 10.0f, "Dragon")]
        [TestCase("WaterKraken", 10.0f, 10.0f, "FireTroll")]
        //Spell Fights
        [TestCase("Spell", 10.0f, 20.0f, "WaterSpell")]
        [TestCase("WaterSpell", 10.0f, 5.0f, "Spell")]
        [TestCase("FireSpell", 10.0f, 5.0f, "WaterSpell")]
        [TestCase("Spell", 10.0f, 5.0f, "FireSpell")]
        [TestCase("FireSpell", 10.0f, 10.0f, "FireSpell")]
        //Mixed Fights
        [TestCase("Knight", 10.0f, 5.0f, "FireSpell")]
        [TestCase("Dragon", 10.0f, 20.0f, "WaterSpell")]
        [TestCase("FireSpell", 15.0f, 7.5f, "WaterWizzard")]
        [TestCase("FireElf", 10.0f, 20.0f, "Spell")]
        [TestCase("WaterSpell", 10.0f, 20.0f, "FireTroll")]
        [TestCase("WaterGoblin", 40.0f, 20.0f, "Spell")]
        //Edge Cases
        [TestCase("Goblin", 10.0f, 0.0f, "Dragon")]
        [TestCase("Ork", 30.0f, 0.0f, "Wizzard")]
        [TestCase("Knight", 25.0f, 0.0f, "WaterSpell")]
        [TestCase("Spell", 3.0f, 0.0f, "Kraken")]
        [TestCase("Dragon", 10.0f, 0.0f, "FireElf")]
        
        public void Card_Attack_DamageEqualsExpectedDamage(string name1, float damage1, float expectedDamage, string name2)
        {
            //Arrange
            Card card1 = new Card();
            card1.Name = name1;
            card1.Damage = damage1;
            card1.FillTypes();

            Card card2 = new Card();
            card2.Name = name2;
            //card2.Damage = damage1;
            card2.FillTypes();

            //Act
            card1.Attack(card2);

            //Assert
            Assert.That(expectedDamage, Is.EqualTo(card1.BattleDamage));
        }

        //TODO: where do you add cards to the db? check for non int/natural values

        /*
        [Test]
        public void Test1()
        {
            Assert.Pass();
        }*/

        /*
        [TestCase("Knight", 1.0f)]
        [TestCase("WaterGoblin", 14.0f)]
        [TestCase("Dragon", 60.0f)]
        [TestCase("FireSpell", 25.0f)]
        [TestCase("WaterWizzard", 25.0f)]
        [TestCase("Kraken", 25.0f)]
        [TestCase("FireElf", 25.0f)]
        [TestCase("Ork", 25.0f)]
        [TestCase("FireTroll", 25.0f)]
        public void Test_FillTypes_DoesntThrowException(string name, float damage)
        {
            //Arrange
            Card card = new Card();
            card.Name = name;
            card.Damage = damage;

            //Act

            //Assert
            Assert.DoesNotThrow(() => card.FillTypes());
        }*/

        /*
        [TestCase("Water", 25.0f)]
        [TestCase("DendroGoblin", 13.0f)]
        [TestCase("WaterGoblin", 10.5f)]
        public void Test_FillTypes_DoesThrowException(string name, float damage)
        {
            //Arrange
            Card card = new Card();
            card.Name = name;
            card.Damage = damage;

            //Act / Assert

            Assert.Throws<Exception>(() => card.FillTypes());
        }*/

        /*
        [Test]
        public void Test_Attack()
        {
            //Arrange
            //Act
            //Assert
            Assert.Pass();
        }*/
    }
    /*
    public class Tests
    {
        //live unit testing?

        //this will all have to be changed, this is just a reference

        /*
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Battle_Exists()
        {
            var battle = new GameHandler();
            Assert.IsNotNull(battle);
        }*/

    /*
    [Test]
    public void Battle_HasOnePlayer()
    {
        var battle = new Battle();
        battle.PlayerOne = new Mock<IPlayer>().Object;

        Assert.IsNotNull(battle.PlayerOne);
    }

}
}

/*
public interface IPlayer
{ }

public class Battle
{
public IPlayer PlayerOne { get; set; }

public Battle() { }
}*/
}