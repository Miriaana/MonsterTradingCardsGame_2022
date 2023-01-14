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
                    AcquirePackage(rq, rs);
                    break;
                default:
                    Console.WriteLine("400 req method not found");
                    rs.ResponseCode = 400;
                    rs.ResponseText = "Bad Request: invalid HttpMethod";
                    break;
            }
        }

        private void AcquirePackage(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Path[1] == null || rq.Path[1] != "packages")
            {
                rs.ResponseCode = 400;
                rs.ResponseText = "Bad Request: invalid path";
            }
            try
            {
                Console.WriteLine("trying to aquire package");
                string mtcgAuth = ((rq.headers["Authorization"]).Split(" "))[1];
                new TransactionHandler().AcquirePackage(mtcgAuth); //change?

                rs.ResponseCode = 200;
                rs.ResponseText = "A package has been successfully bought";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.Message.StartsWith("401"))
                {
                    rs.ResponseCode = 401;
                    rs.ResponseText = "Access token is missing or invalid";
                }
                else if (ex.Message.StartsWith("403"))
                {
                    rs.ResponseCode = 403;
                    rs.ResponseText = "Not enough money for buying a card package";
                }
                else if (ex.Message.StartsWith("404"))
                {
                    rs.ResponseCode = 404;
                    rs.ResponseText = "No card package available for buying";
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
