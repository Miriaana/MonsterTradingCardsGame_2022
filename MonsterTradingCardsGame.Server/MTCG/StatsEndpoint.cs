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
    public class StatsEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            switch (rq.Method)
            {
                case EHttpMethod.GET:
                    GetStats(rq, rs);
                    break;
                default:
                    Console.WriteLine("404 req method not found"); //change: return error or set rs
                    break;
            }
        }

        private void GetStats(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                string mtcgAuth = rq.GetToken();
                Stats stats = new StatHandler().GetStats(mtcgAuth);

                rs.ResponseCode = 200;
                rs.ResponseText = "The stats could be retrieved successfully.";
                rs.Headers["Content-Type"] = "application/javascript";
                rs.ResponseContent = JsonSerializer.Serialize(stats);
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
