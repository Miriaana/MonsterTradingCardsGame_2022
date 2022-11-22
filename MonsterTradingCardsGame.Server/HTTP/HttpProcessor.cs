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

        public HttpProcessor(TcpClient clientSocket)
        {
            this.clientSocket = clientSocket;
        }

        public void run()
        {
            Console.WriteLine($"    {Thread.CurrentThread.ManagedThreadId}: started processing");
            var reader = new StreamReader(clientSocket.GetStream());
            var request = new HttpRequest(reader);

            var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = true };
            var response = new HttpResponse(writer);

            request.Parse();

            //Verarbeitung und Aufrufen des BLs
            //...unterschiedliche Endpunkte
            //...application spezifische verarbeitung

            //todo wth dict refernce auf controller mappen
            //todo: add different paths
            if (request.Path.Equals("/users"))
            {
                UserController userController = new UserController();
                userController.createUser(request, response);

            }
            else
            {
                response.ResponseCode = 404;                    //200 //TODO: also enum
                response.ResponseText = "Path not found";       //"OK"
                response.ResponseContent = "<html><body>Hello World!</body></html>";
                response.Headers.Add("Content-Length", response.ResponseContent.Length.ToString());
                response.Headers.Add("Content-Type", "text/plain"); //application/json 
            }
            /*
            Thread.Sleep(5000);
            Console.WriteLine($"    {Thread.CurrentThread.ManagedThreadId}: sleeping"); Console.Out.Flush();
            Thread.Sleep(5000);
            */
            
            response.Process();
            Console.WriteLine($"    {Thread.CurrentThread.ManagedThreadId}: finished processing"); Console.Out.Flush();
        }
    }
}
