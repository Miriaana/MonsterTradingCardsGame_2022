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
    public class TransactionsEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            switch (rq.Method)
            {
                case EHttpMethod.POST:
                    CreateSession(rq, rs);
                    break;
                default:
                    Console.WriteLine("404 req method not found"); //change: return error or set rs
                    break;
            }
        }

        private void CreateSession(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = JsonSerializer.Deserialize<User>(rq.Content);//note: move user to model
                // call BL
                new UserHandler().CreateUser(user); //change?

                rs.ResponseCode = 200;
                rs.ResponseText = "User login successful";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.Message.StartsWith("0"))
                {
                    rs.ResponseCode = 401;
                    rs.ResponseContent = "Invalid username/password provided";
                }
                else
                {
                    rs.ResponseCode = 500;
                    rs.ResponseContent = "Internal Server Error";
                }

            }
        }
    }
}
