using MonsterTradingCardsGame.BL;
using MonsterTradingCardsGame.Model;
using MTCGame.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                case EHttpMethod.GET:
                    GetUser(rq, rs);
                    break;
                case EHttpMethod.PUT:
                    UpdateUser(rq, rs); 
                    break;
                default:
                    Console.WriteLine("404 req method not found"); //change: return error or set rs
                    break;
            }
        }

        private void CreateUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                Console.WriteLine($"1 {rq.Content}");
                var user = JsonSerializer.Deserialize<User>(rq.Content);//note: move user to model
                Console.WriteLine($"2");
                // call BL
                new UserHandler().CreateUser(user); //change?

                rs.ResponseCode = 201;
                rs.ResponseText = "User successfully created";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if(ex.Message.StartsWith("23505"))
                {
                    rs.ResponseCode = 409;
                    rs.ResponseContent = "User with same username already registered";
                }
                else
                {
                    rs.ResponseCode = 500;
                    rs.ResponseContent = "Internal Server Error";
                }
                
            }
        }

        //allows user to get their own user information
        private void GetUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = new User(rq.Path[1]);
                user.Token = rq.headers["Authorization"];
                // call BL
                user = (new UserHandler()).GetUser(user); //change?

                rs.ResponseCode = 200;
                rs.ResponseText = "Data successfully retrieved";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.Message.StartsWith("0"))
                {
                    rs.ResponseCode = 401;
                    rs.ResponseContent = "Access token is missing or invalid";
                }else if (ex.Message.StartsWith("1"))
                {
                    rs.ResponseCode = 404;
                    rs.ResponseContent = "User not found.";
                }
                else
                {
                    rs.ResponseCode = 500;
                    rs.ResponseContent = "Internal Server Error";
                }

            }
        }

        private void UpdateUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = new User(rq.Path[1], "");
                user.Token = rq.headers["Authorization"];
                new UserHandler().UpdateUserProfile(user); //change?

                rs.ResponseCode = 201;
                rs.ResponseText = "User successfully created";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.Message.StartsWith("23505"))
                {
                    rs.ResponseCode = 409;
                    rs.ResponseContent = "User with same username already registered";
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
