﻿using MonsterTradingCardsGame.BL;
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
    public class SessionsEndpoint : IHttpEndpoint
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
                //string token = (new SessionHandler()).CreateSession(user); //change?

                //rs.ResponseContent = token;
                rs.ResponseCode = 200;
                rs.ResponseText = "User login successful";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.Message.StartsWith("0"))
                {
                    rs.ResponseCode = 401;
                    rs.ResponseText = "Invalid username/password provided";
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