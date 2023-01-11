using MonsterTradingCardsGame.BL;
using MonsterTradingCardsGame.Model;
using MTCGame.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCGame.Server.MTCG
{
    public class UsersEndpoint : IHttpEndpoint
    {

        public void HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            switch (rq.Method)
            {
                case EHttpMethod.POST:
                    CreateUser(rq, rs);
                    break;
                /*
                case EHttpMethod.GET:
                    GetUsers(rq, rs);
                    break;*/
                default:
                    Console.WriteLine("404 req method not found"); //change: return error or set rs
                    break;
            }
        }

        private void CreateUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = JsonSerializer.Deserialize<User>(rq.Content);//note: move user to model
                // call BL
                new UserHandler().CreateUser(user); //change?

                rs.ResponseCode = 201;
                rs.ResponseText = "OK";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                rs.ResponseCode = 400;
                rs.ResponseContent = "Failed to create User! ";
            }
        }

        private void GetUsers(HttpRequest rq, HttpResponse rs)
        {
            // BL 
            List<User> users = new List<User>();
            users.Add(new User("Rudi Ratlos", "1234"));
            users.Add(new User("Susi Sorglos", "0000"));

            rs.ResponseContent = JsonSerializer.Serialize(users);
            rs.Headers.Add("Content-Type", "application/json");
            rs.ResponseCode = 200;
            rs.ResponseText = "OK";
        }

    }
}
