using MTCGame.BL;
using MTCGame.Model;
using MTCGame.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCGame.Server.MTCG
{
    public class CardsEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            switch (rq.Method)
            {
                case EHttpMethod.GET:
                    GetCards(rq, rs);
                    break;
                default:
                    Console.WriteLine("400 req method not found");
                    rs.ResponseCode = 400;
                    rs.ResponseText = "Bad Request: invalid HttpMethod";
                    break;
            }
        }

        private void GetCards(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                string mtcgAuth = rq.GetToken();
                List<Card> stack = new CardHandler().GetStack(mtcgAuth); //change?

                rs.ResponseCode = 200;
                rs.ResponseText = " The user has cards, the response contains these";
                rs.Headers["Content-Type"] = "application/javascript";
                rs.ResponseContent = JsonSerializer.Serialize(stack);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.Message.StartsWith("204"))
                {
                    rs.ResponseCode = 204;
                    rs.ResponseText = "The request was fine, but the user doesn't have any cards";
                }
                else if (ex.Message.StartsWith("401"))
                {
                    rs.ResponseCode = 401;
                    rs.ResponseText = "Access token is missing or invalid";
                }
                else
                {
                    rs.ResponseCode = 500;
                    rs.ResponseText = "Internal Server Error";
                }

            }
        }
    }
}

