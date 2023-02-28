using MTCGame.BL;
using MTCGame.Model;
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
        public void CreateUser()
        {
            throw new NotImplementedException();
        }

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
                    Console.WriteLine("400 req method not found");
                    rs.ResponseCode = 400;
                    rs.ResponseText = "Bad Request: invalid HttpMethod";
                    break;
            }
        }

        private void CreateUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = JsonSerializer.Deserialize<User>(rq.Content);

                new UserHandler().CreateUser(user);

                rs.ResponseCode = 201;
                rs.ResponseText = "User successfully created";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if(ex.Message.StartsWith("23505"))
                {
                    rs.ResponseCode = 409;
                    rs.ResponseText = "User with same username already registered";
                }
                else
                {
                    rs.ResponseCode = 500;
                    rs.ResponseText = "Internal Server Error";
                }
                
            }
        }

        //allows user to get their own user information
        private void GetUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = new User(rq.Path[1]);
                string mtcgAuth = ((rq.headers["Authorization"]).Split(" "))[1];

                user = (new UserHandler()).GetUser(mtcgAuth, user);

                rs.ResponseCode = 200;
                rs.ResponseText = "Data successfully retrieved";
                rs.Headers["Content-Type"] = "application/javascript";
                rs.ResponseContent = JsonSerializer.Serialize(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.Message.StartsWith("0"))
                {
                    rs.ResponseCode = 401;
                    rs.ResponseText = "Access token is missing or invalid";
                }else if (ex.Message.StartsWith("1"))
                {
                    rs.ResponseCode = 404;
                    rs.ResponseText = "User not found.";
                }
                else
                {
                    rs.ResponseCode = 500;
                    rs.ResponseText = "Internal Server Error";
                }

            }
        }

        //change profile
        private void UpdateUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = JsonSerializer.Deserialize<User>(rq.Content);
                //var user = new User(rq.Path[1]);
                string mtcgAuth = rq.GetToken();
                new UserHandler().UpdateUserProfile(mtcgAuth, user); //change?

                rs.ResponseCode = 201;
                rs.ResponseText = "User successfully created";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.Message.StartsWith("0"))
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
