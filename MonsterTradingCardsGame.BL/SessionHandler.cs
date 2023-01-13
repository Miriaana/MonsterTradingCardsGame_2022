using MTCGame.DAL;
using MTCGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.BL
{
    public class SessionHandler
    {
        public string CreateSession(User user)
        {
            var repo = new PostgreSQLRepository();

            return repo.CreateSession(user);
        }
    }
}
