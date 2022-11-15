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
        private readonly int port = 10001;
        private readonly IPAddress ipAddress;

        private TcpListener httpListener;

        public HttpServer(IPAddress adr, int port)
        {
            ipAddress = adr;
            this.port = port; //change?!?

            httpListener = new TcpListener(ipAddress, port);
        }

        public void run()
        {
            httpListener.Start();
            while (true)
            {
                Console.WriteLine("[1] Waiting for new client request..."); Console.Out.Flush();
                var clientSocket = httpListener.AcceptTcpClient();
                var httpProcessor = new HttpProcessor(clientSocket);
                Console.WriteLine("[2] New Client accepted"); Console.Out.Flush();
                //what about "keep alive" browser connections -> handle somehow?
                Task.Factory.StartNew(() =>
                {
                    httpProcessor.run();
                });
                Console.WriteLine("[3] Client Task started"); Console.Out.Flush();
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
