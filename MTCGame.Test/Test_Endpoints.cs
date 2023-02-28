//using MTCGame.Server.HTTP;
using Moq;
using MTCGame.BL;
using MTCGame.DAL;
using MTCGame.Model;
using MTCGame.Server.HTTP;
using MTCGame.Server.MTCG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Test
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
    }
}
