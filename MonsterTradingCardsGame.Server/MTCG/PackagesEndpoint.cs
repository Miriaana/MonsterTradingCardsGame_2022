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
    public class PackagesEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            switch (rq.Method)
            {
                case EHttpMethod.POST:
                    CreatePackage(rq, rs);
                    break;
                default:
                    Console.WriteLine("400 req method not found");
                    rs.ResponseCode = 400;
                    rs.ResponseText = "Bad Request: invalid HttpMethod";
                    break;
            }
        }

        private void CreatePackage(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                //var package = JsonConvert.DeserializeObject<List<Card>>(rq.Content);
                var package = JsonSerializer.Deserialize<List<Card>>(rq.Content);
                string mtcgAuth = rq.GetToken();

                new PackageHandler().CreatePackage(mtcgAuth, package);

                rs.ResponseCode = 201;
                rs.ResponseText = "Package and cards successfully created";
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
                    rs.ResponseText = "Provided user is not \"admin\"";
                }
                else if (ex.Message.StartsWith("409"))
                {
                    rs.ResponseCode = 409;
                    rs.ResponseText = "At least one card in the packages already exists";
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
