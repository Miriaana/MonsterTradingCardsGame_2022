using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Server
{
    internal class HttpProcessor
    {
        private TcpClient clientSocket;

        public HttpProcessor(TcpClient clientSocket)
        {
            this.clientSocket = clientSocket;
        }

        public void run()
        {
            var reader = new StreamReader(clientSocket.GetStream());
            var request = new HttpRequest(reader);
            request.Parse();

            Thread.Sleep(10000);

            var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = true };
            var response = new HttpResponse(writer);
            response.ResponseCode = 200;
            response.ResponseText = "OK";
            response.ResponseContent = "<html><body>Hello World!</body></html>";
            response.Process();
        }
    }
}
