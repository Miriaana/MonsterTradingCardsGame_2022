using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Server.HTTP
{
    public interface IHttpEndpoint
    {
        void HandleRequest(HttpRequest rq, HttpResponse rs);
    }
}
