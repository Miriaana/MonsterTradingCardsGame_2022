using MTCGame.Server.MTCG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Server.HTTP
{
    internal class HttpProcessor
    {
        private TcpClient clientSocket;
        private HttpServer httpServer;

        public HttpProcessor(HttpServer httpServer, TcpClient clientSocket)
        {
            this.httpServer = httpServer;
            this.clientSocket = clientSocket;
        }

        public void run()
        {
            //Console.WriteLine($"    {Thread.CurrentThread.ManagedThreadId}: started processing");

            var reader = new StreamReader(clientSocket.GetStream());
            var request = new HttpRequest(reader);

            var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = true };
            var response = new HttpResponse(writer);

            try
            {
                request.Parse();

                IHttpEndpoint endpoint;
                httpServer.Endpoints.TryGetValue(request.Path[0], out endpoint);
                if (endpoint != null)
                {
                    endpoint.HandleRequest(request, response);
                }
                else
                {
                    //Thread.Sleep(10000);
                    response.ResponseCode = 404;
                    response.ResponseText = "Not Found";
                    response.ResponseContent = "<html><body>Path not found!</body></html>";
                    response.Headers.Add("Content-Length", response.ResponseContent.Length.ToString());
                    response.Headers.Add("Content-Type", "text/html"); // application/json
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.ResponseCode = 500;
                response.ResponseText = "Internal Server Error";
                response.ResponseContent = "<html><body>Hello World!</body></html>";
            }

            response.Process();
            //Console.WriteLine($"    {Thread.CurrentThread.ManagedThreadId}: finished processing"); Console.Out.Flush();
            /*
            
            finally 
            {
                response.Process();
                Console.WriteLine($"    {Thread.CurrentThread.ManagedThreadId}: finished processing"); Console.Out.Flush();
            }*/


            /*
            Thread.Sleep(5000);
            Console.WriteLine($"    {Thread.CurrentThread.ManagedThreadId}: sleeping"); Console.Out.Flush();
            Thread.Sleep(5000);
            */
        }
    }
}
