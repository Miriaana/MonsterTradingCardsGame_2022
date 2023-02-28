using MTCGame.Model;
//using Moq;
using MTCGame.BL;
//using static System.Net.Mime.MediaTypeNames;
//using System.Xml;
using MTCGame.Server.HTTP;
//using Newtonsoft.Json.Linq;
using System.Text;
using System;
using System.Collections.Generic;

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
        [TestCase("FireTroll", 0.0f)]
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

        [TestCase("WaterSpell", MajorCardType.Spell)]
        [TestCase("FireElf", MajorCardType.Monster)]
        [TestCase("WaterGoblin", MajorCardType.Monster)]
        [TestCase("Kraken", MajorCardType.Monster)]
        public void Card_FillTypes_FillMajorTypeAsExpected(string name, MajorCardType expectedType)
        {
            Card card = new Card();
            card.Name = name;

            card.FillTypes();

            Assert.That(card.MajorType, Is.EqualTo(expectedType));
        }

        [TestCase("WaterSpell", CardElement.water)]
        [TestCase("FireElf", CardElement.fire)]
        [TestCase("WaterGoblin", CardElement.water)]
        [TestCase("Kraken", CardElement.regular)]
        public void Card_FillTypes_FillElementAsExpected(string name, CardElement expectedElement)
        {
            Card card = new Card();
            card.Name = name;

            card.FillTypes();

            Assert.That(card.Element, Is.EqualTo(expectedElement));
        }

        [TestCase("WaterSpell", MinorCardType.Spell)]
        [TestCase("FireElf", MinorCardType.Elf)]
        [TestCase("WaterGoblin", MinorCardType.Goblin)]
        [TestCase("Knight", MinorCardType.Knight)]
        public void Card_FillTypes_FillMinorTypeAsExpected(string name, MinorCardType expectedType)
        {
            Card card = new Card();
            card.Name = name;

            card.FillTypes();

            Assert.That(card.MinorType, Is.EqualTo(expectedType));
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

        [TestCase(CardElement.regular, CardElement.regular, 0)]
        [TestCase(CardElement.water, CardElement.water, 0)]
        [TestCase(CardElement.fire, CardElement.fire, 0)]
        [TestCase(CardElement.regular, CardElement.water, 1)]
        [TestCase(CardElement.regular, CardElement.fire, -1)]
        [TestCase(CardElement.water, CardElement.fire, 1)]
        [TestCase(CardElement.water, CardElement.regular, -1)]
        [TestCase(CardElement.fire, CardElement.regular, 1)]
        [TestCase(CardElement.fire, CardElement.water, -1)]
        public void Card_IsEffectiveAgainst_ReturnsExpectedValue(CardElement element, CardElement enemyElement, int expectedEffectiveness)
        {
            Card card = new Card();
            card.Element = element;

            Assert.That(card.IsEffectiveAgainst(enemyElement), Is.EqualTo(expectedEffectiveness));
        }


        [TestCase("PUT /deck HTTP/1.1\nHost: localhost:10001\nUser-Agent: curl/7.83.1\nAccept: */*\nContent-Type: application/json\nAuthorization: Bearer altenhof-mtcgToken\nContent-Length: 160\n\n[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\"]")]
        [TestCase("PUT /sessions HTTP/1.1\nHost: localhost:10001\nUser-Agent: curl/7.83.1\nAccept: */*\nContent-Type: application/json")]
        [TestCase("POST /users HTTP/1.1\nHost: localhost:10001\nUser-Agent: curl/7.83.1\nAccept: */*\nContent-Type: application/json\nContent-Length: 46\n\n{\"Username\":\"admin\",    \"Password\":\"istrator\"}")]
        public void HttpRequest_Parse_DoesNotThrowException(string lines)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(lines ?? ""));
            var reader = new StreamReader(stream); //clientSocket.GetStream()
            var rq = new HttpRequest(reader);

            Assert.DoesNotThrow(rq.Parse);


        }

        [TestCase("hello world", "400: Invalid Http Request")]
        [TestCase("UPDATE /deck HTTP/1.1\nHost: localhost:10001\nUser-Agent: curl/7.83.1\nAccept: */*\nContent-Type: application/json\nAuthorization: Bearer altenhof-mtcgToken\nContent-Length: 160\n\n[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\"]",
            "400: Invalid Http Request")]

        public void HttpRequest_Parse_DoesThrowException(string lines, string expectedMessage)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(lines ?? ""));
            var reader = new StreamReader(stream);
            var rq = new HttpRequest(reader);

            var ex = Assert.Throws<Exception>(rq.Parse);

            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        [TestCase("PUT /deck HTTP/1.1\nHost: localhost:10001\nUser-Agent: curl/7.83.1\nAccept: */*\nContent-Type: application/json\nAuthorization: Bearer altenhof-mtcgToken\nContent-Length: 160\n\n[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\"]",
            EHttpMethod.PUT)]
        [TestCase("GET /deck HTTP/1.1\nHost: localhost:10001\nUser-Agent: curl/7.83.1\nAccept: */*\nContent-Type: application/json\nAuthorization: Bearer altenhof-mtcgToken\nContent-Length: 160\n\n[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\"]",
            EHttpMethod.GET)]
        public void HttpRequest_Parse_PropertyMethodHasExpectedValue(string lines, EHttpMethod expectedMethod)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(lines ?? ""));
            var reader = new StreamReader(stream);
            var rq = new HttpRequest(reader);

            rq.Parse();

            Assert.That(rq.Method, Is.EqualTo(expectedMethod));
        }

        [TestCase("PUT /deck HTTP/1.1\nHost: localhost:10001\nUser-Agent: curl/7.83.1\nAccept: */*\nContent-Type: application/json\nAuthorization: Bearer altenhof-mtcgToken\nContent-Length: 160\n\n[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\"]",
            "HTTP/1.1")]
        public void HttpRequest_Parse_PropertyProtocolVersionHasExpectedValue(string lines, string expectedVersion)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(lines ?? ""));
            var reader = new StreamReader(stream);
            var rq = new HttpRequest(reader);

            rq.Parse();

            Assert.That(rq.ProtocolVersion, Is.EqualTo(expectedVersion));
        }

        [TestCase("PUT /deck HTTP/1.1\nHost: localhost:10001\nUser-Agent: curl/7.83.1\nAccept: */*\nContent-Type: application/json\nAuthorization: Bearer altenhof-mtcgToken\nContent-Length: 160\n\n[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\"]",
            "[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\"]")]
        public void HttpRequest_Parse_PropertyContentHasExpectedValue(string lines, string expectedContent)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(lines ?? ""));
            var reader = new StreamReader(stream);
            var rq = new HttpRequest(reader);

            rq.Parse();

            Assert.That(rq.Content, Is.EqualTo(expectedContent));

        }

        [TestCase("PUT /deck HTTP/1.1\nHost: localhost:10001\nUser-Agent: curl/7.83.1\nAccept: */*\nContent-Type: application/json\nAuthorization: Bearer altenhof-mtcgToken\nContent-Length: 160\n\n[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\"]"
            , new[] { "deck" })]
        public void HttpRequest_Parse_PropertyPathHasExpectedValue(string lines, string[] arrayExpectedPath)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(lines ?? ""));
            var reader = new StreamReader(stream);
            var rq = new HttpRequest(reader);

            List<string> expectedPath = arrayExpectedPath.ToList();

            rq.Parse();

            Assert.That(expectedPath, Is.EqualTo(rq.Path));

        }

        [TestCase("PUT /deck HTTP/1.1\nHost: localhost:10001\nUser-Agent: curl/7.83.1\nAccept: */*\nContent-Type: application/json\nAuthorization: Bearer altenhof-mtcgToken\nContent-Length: 160\n\n[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\"]"
            , new[] { "Host", "localhost:10001", "User-Agent", "curl/7.83.1", "Accept", "*/*", "Content-Type", "application/json", "Authorization", "Bearer altenhof-mtcgToken", "Content-Length", "160" })]
        public void HttpRequest_Parse_PropertyHeadersHasExpectedValue(string lines, string[] arrayExpectedHeaders)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(lines ?? ""));
            var reader = new StreamReader(stream);
            var rq = new HttpRequest(reader);

            rq.Parse();

            for (int i = 0; i < arrayExpectedHeaders.Length; i = i + 2)
            {
                Assert.That(rq.headers, Does.ContainKey(arrayExpectedHeaders[i]).WithValue(arrayExpectedHeaders[i + 1]));
            }
        }


        [TestCase("GET /deck?format=plain HTTP/1.1\nHost: localhost:10001\nUser-Agent: curl/7.83.1\nAccept: */*\nAuthorization: Bearer altenhof-mtcgToken"
            , new[] { "format", "plain" })]
        public void HttpRequest_Parse_PropertyQueryParamsHasExpectedValue(string lines, string[] arrayQueryParams)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(lines ?? ""));
            var reader = new StreamReader(stream);
            var rq = new HttpRequest(reader);

            rq.Parse();

            Assert.That(rq.QueryParams.Count, Is.EqualTo(arrayQueryParams.Length / 2));

            for (int i = 0; i < arrayQueryParams.Length; i = i + 2)
            {
                Assert.That(rq.QueryParams, Does.ContainKey(arrayQueryParams[i]).WithValue(arrayQueryParams[i + 1]));
            }

        }
    }
}










//references and attempted tests ->



    /*
            [Test]
            public void Test1()
            {
                Assert.Pass();
            }*/

    /*namespace MTCGame.Test
{
    internal class Test_Endpoints
    {
        /*
        [TestCase(EHttpMethod.POST)]
        public void UsersEndpoint_CreateUser_ValidInputResponse(EHttpMethod method)
        {
            //var stream = new MemoryStream(Encoding.UTF8.GetBytes(lines ?? ""));
            //var reader = new StreamReader(stream); //clientSocket.GetStream()
            var rs = new Mock<HttpResponse>().Object;
            var rqMock = new Mock<HttpRequest>();

            //rqMock.Setup(x => x.Method
            //rqMock.Setup(rq => rq.Method).Returns(method);
            rqMock.SetupProperty(rq => rq.Method, method);
            HttpRequest rq = rqMock.Object;
            //var rs = new HttpResponse(reader);
            var endpoint = new UsersEndpoint();
            endpoint.HandleRequest(rq, rs);
            //Assert.That(rs.ResponseCode, Is.EqualTo(200));
            //Assert.IsCalled
        }

        [TestCase(EHttpMethod.POST)]
        public void UsersEndpoint_HandleRequest(EHttpMethod method)
        {
            var endpointMock = new Mock<UsersEndpoint>();
            //endpointMock.Setup(endpoint => endpoint.).Returns(0);
            //HttpRequest rq = rqMock.Object;
            //var rs = new HttpResponse(reader);
            
            var endpoint = new UsersEndpoint();
            //endpoint.HandleRequest(rq, rs);
            //Assert.That(rs.ResponseCode, Is.EqualTo(200));
            //Assert.IsCalled
        }

        //mock.Verify(foo => foo.DoSomething("ping"), Times.AtLeastOnce());

        [Test]
        public void DeckHandler_GetDeck_throwsAuthException()
        {
            string invalidToken = "abdfc";
            var repoMock = new Mock<PostgreSQLRepository>();
            repoMock.Setup(repo => repo.CheckAuthorization(invalidToken)).Returns("");

            var deckHandlerMock = new Mock<DeckHandler>();


        }



            public List<Card> GetDeck(string mtcgAuth)
        {
            var repo = new PostgreSQLRepository();

            string auth = repo.CheckAuthorization(mtcgAuth);
            if (string.IsNullOrEmpty(auth))
            {
                throw new Exception("401: Access token is missing or invalid");
            }
            List<Card> deck = repo.GetDeck(auth);
            if (deck.Count == 0)
            {
                throw new Exception("204: The request was fine, but the deck doesn't have any cards");
            }
            return deck;
        }*/
    /*
}
}
*/
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
