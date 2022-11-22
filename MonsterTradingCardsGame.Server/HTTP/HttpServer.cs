using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Server.HTTP
{
    public class HttpServer
    {
        private readonly int port; //= 10001
        private readonly IPAddress ipAddress;

        private TcpListener httpListener;

        public HttpServer(IPAddress adr, int port)
        {
            ipAddress = adr;
            this.port = port;

            httpListener = new TcpListener(ipAddress, this.port);
        }

        public void run()
        {
            httpListener.Start();
            while (true)
            {
                Console.WriteLine("[1] Waiting for new client request...");
                var clientSocket = httpListener.AcceptTcpClient();
                var httpProcessor = new HttpProcessor(clientSocket);
                Console.WriteLine("[2] New Client accepted");

                //what about "keep alive" browser connections -> handle somehow?
                Task.Factory.StartNew(() =>
                {
                    httpProcessor.run();
                });
                Console.WriteLine("[3] Client Task started"); //Console.Out.Flush();
            }
        }
    }
}
