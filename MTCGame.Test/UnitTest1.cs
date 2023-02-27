using MTCGame.Model;
//using Moq;
using MTCGame.BL;

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
        [TestCase("", "", "\n\n")]
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