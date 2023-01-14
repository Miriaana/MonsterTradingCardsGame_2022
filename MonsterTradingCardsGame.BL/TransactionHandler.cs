using MTCGame.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.BL
{
    public class TransactionHandler
    {
        public void AcquirePackage(string mtcgAuth)
        {
            var repo = new PostgreSQLRepository();
            string auth = repo.CheckAuthorization(mtcgAuth);

            if (string.IsNullOrEmpty(auth))
            {
                throw new Exception("401: Access token is missing or invalid");
            }

            repo.AcquirePackage(auth);
        }
    }
}
