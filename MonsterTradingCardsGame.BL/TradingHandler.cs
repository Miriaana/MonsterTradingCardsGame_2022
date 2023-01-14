using MTCGame.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.BL
{
    public class TradingHandler
    {
        public void AcquirePackage(string mtcgAuth)
        {
            var repo = new PostgreSQLRepository();
            Console.WriteLine("trying to authorize");
            string auth = repo.CheckAuthorization(mtcgAuth);
            Console.WriteLine("more trying to authorize");
            if (string.IsNullOrEmpty(auth))
            {
                throw new Exception("401: Access token is missing or invalid");
            }
            Console.WriteLine("authorizing complete");

            repo.AcquirePackage(auth);
        }
    }
}
