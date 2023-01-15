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
    public class BattlesEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            switch (rq.Method)
            {
                case EHttpMethod.POST:
                    StartBattle(rq, rs);
                    break;
                default:
                    Console.WriteLine("400 req method not found");
                    rs.ResponseCode = 400;
                    rs.ResponseText = "Bad Request: invalid HttpMethod";
                    break;
            }
        }

        private void StartBattle(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                string mtcgAuth = rq.GetToken();
                string battleLog = new BattleHandler().StartBattle(mtcgAuth); //change?

                rs.ResponseCode = 200;
                rs.ResponseText = "User login successful";
                rs.Headers["Content-Type"] = "text/plain";
                rs.ResponseContent = battleLog;
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
