using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Server
{
    public class HttpServer
    {
        private readonly int port = 10001;
        private readonly IPAddress ipAddress;

        private TcpListener httpListener;

        public HttpServer(IPAddress adr, int port)
        {
            this.ipAddress = adr;
            this.port = port; //change?!?

            httpListener = new TcpListener(ipAddress, port);
        }

        public void run()
        {
            httpListener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for new client request...");
                var clientSocket = httpListener.AcceptTcpClient();
                var httpProcessor = new HttpProcessor(clientSocket);
                Task.Factory.StartNew(() =>
                {
                    httpProcessor.run();
                });
            }
        }
        /*
        start up and set up

        wait for client
            ->create new async task
        keep waiting for client in main

        stop server (manage unfinished tasks!)
        ---
        async task(message)
            analyze http message
            valid request?
                no -> send back appropriate response
                yes -> do the thing, if battle keep waiting

            
         */
    }
}
