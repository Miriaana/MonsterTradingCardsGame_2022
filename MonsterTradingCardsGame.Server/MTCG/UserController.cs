using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MonsterTradingCardsGame.DAL;
using MonsterTradingCardsGame.Model;
using MTCGame.Server.HTTP;

namespace MTCGame.Server.MTCG
{
    public class UserController : IHttpEndpoint
    {
        public void createUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = JsonSerializer.Deserialize<User>(rq.Content);
                var repo = new PostgreSQLRepository();
                //repo.CreateUser(user);
                
                rs.ResponseCode = 201;
                rs.ResponseContent = "Successfully created User!";
            }
            catch (Exception)
            {
                rs.ResponseCode = 400;
                rs.ResponseContent = "Failed to create User!";
            }

        }

        public void HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = JsonSerializer.Deserialize<User>(rq.Content);
                rs.ResponseCode = 201;
                rs.ResponseContent = "Successfully created User!";
            }
            catch (Exception)
            {
                rs.ResponseCode = 400;
                rs.ResponseContent = "Failed to create User!";
            }
        }
    }
}
