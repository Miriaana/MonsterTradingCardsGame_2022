using MTCGame.DAL;
using MTCGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.BL
{
    public class PackageHandler
    {
        public void CreatePackage(string mtcgAuth, List<Card> package)
        {
            var repo = new PostgreSQLRepository();
            string auth = repo.CheckAuthorization(mtcgAuth);

            if (auth != "admin")
            {
                throw new Exception("403: Provided user is not \"admin\"");
            }

            if (package.Count != 5)
            {
                throw new Exception("422: Wrong Amount of Cards in Package");
            }

            repo.CreatePackage(package);
            Console.WriteLine("Is there sth wrong here?");
        }
    }
}
