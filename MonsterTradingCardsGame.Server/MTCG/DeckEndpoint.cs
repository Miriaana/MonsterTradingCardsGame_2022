using MTCGame.BL;
using MTCGame.Model;
using MTCGame.Server.HTTP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCGame.Server.MTCG
{
    public class DeckEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            switch (rq.Method)
            {
                case EHttpMethod.GET:
                    GetDeck(rq, rs);
                    break;
                case EHttpMethod.PUT:
                    ConfigureDeck(rq, rs);
                    break;
                default:
                    Console.WriteLine("400 req method not found");
                    rs.ResponseCode = 400;
                    rs.ResponseText = "Bad Request: invalid HttpMethod";
                    break;
            }
        }

        private void GetDeck(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                string mtcgAuth = rq.GetToken();
                List<Card> deck = new DeckHandler().GetDeck(mtcgAuth);

                rs.ResponseCode = 200;
                rs.ResponseText = "The deck has cards, the response contains these";
                rs.Headers["Content-Type"] = "application/javascript";
                rs.ResponseContent = JsonSerializer.Serialize(deck);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.Message.StartsWith("204"))
                {
                    rs.ResponseCode = 204;
                    rs.ResponseText = "The request was fine, but the deck doesn't have any cards";
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

        private void ConfigureDeck(HttpRequest rq, HttpResponse rs)
        {
            Console.WriteLine($"endpoint configure deck");
            try
            {
                string mtcgAuth = rq.GetToken();
                if(rq.Content == null || rq.Content.Length == 0)
                {
                    throw new Exception("400: The provided deck did not include the required amount of cards");
                }
                List<string> cardids = JsonSerializer.Deserialize<List<string>>(rq.Content);
                if (cardids.Count != 4)
                {
                    throw new Exception("400: The provided deck did not include the required amount of cards");
                }
                new DeckHandler().ConfigureDeck(mtcgAuth, cardids);

                rs.ResponseCode = 200;
                rs.ResponseText = "The deck has been successfully configured";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.Message.StartsWith("400"))
                {
                    rs.ResponseCode = 400;
                    rs.ResponseText = "The provided deck did not include the required amount of cards";
                }
                else if (ex.Message.StartsWith("401"))
                {
                    rs.ResponseCode = 401;
                    rs.ResponseText = "Access token is missing or invalid";
                }
                else if (ex.Message.StartsWith("403"))
                {
                    rs.ResponseCode = 403;
                    rs.ResponseText = "At least one of the provided cards does not belong to the user or is not available.";
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
