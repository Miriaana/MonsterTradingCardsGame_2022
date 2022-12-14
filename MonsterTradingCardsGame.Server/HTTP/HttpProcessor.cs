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

            try
            {
                request.Parse();

                if (request.Path[0].Equals("users"))
                {
                    UserController userController = new UserController();
                    userController.createUser(request, response);
                    //if(/someusername)
                }
                else if (request.Path[0].Equals("sessions"))
                {
                    response.ResponseCode = 200; response.ResponseText = "OK";
                    response.ResponseContent = "Path /sessions";
                    Console.WriteLine(response.ResponseContent);
                }
                else if (request.Path[0].Equals("packages"))
                {
                    response.ResponseCode = 200; response.ResponseText = "OK";
                    response.ResponseContent = "Path /packages";
                    Console.WriteLine(response.ResponseContent);
                }
                else if (request.Path[0].Equals("transactions"))
                {
                    // additional path packages
                    response.ResponseCode = 200; response.ResponseText = "OK";
                    response.ResponseContent = "Path /transactions";
                    Console.WriteLine(response.ResponseContent);
                    //if(/packages)
                }
                else if (request.Path[0].Equals("cards"))
                {
                    response.ResponseCode = 200; response.ResponseText = "OK";
                    response.ResponseContent = "Path /cards";
                    Console.WriteLine(response.ResponseContent);
                }
                else if (request.Path[0].Equals("deck"))
                {
                    response.ResponseCode = 200; response.ResponseText = "OK";
                    response.ResponseContent = "Path /deck";
                    Console.WriteLine(response.ResponseContent);
                    //if(? format = plain)
                }
                else if (request.Path[0].Equals("stats"))
                {
                    response.ResponseCode = 200; response.ResponseText = "OK";
                    response.ResponseContent = "Path /stats";
                    Console.WriteLine(response.ResponseContent);
                    //if(? format = plain)
                }
                else if (request.Path[0].Equals("score"))
                {
                    response.ResponseCode = 200; response.ResponseText = "OK";
                    response.ResponseContent = "Path /score";
                    Console.WriteLine(response.ResponseContent);
                    //if(? format = plain)
                }
                else if (request.Path[0].Equals("battles"))
                {
                    response.ResponseCode = 200; response.ResponseText = "OK";
                    response.ResponseContent = "Path /deck";
                    Console.WriteLine(response.ResponseContent);
                    //if(? format = plain)
                }
                else if (request.Path[0].Equals("tradings"))
                {
                    response.ResponseCode = 200; response.ResponseText = "OK";
                    response.ResponseContent = "Path /deck";
                    Console.WriteLine(response.ResponseContent);
                    //if(? format = plain)
                }
                else
                {
                    response.ResponseCode = 404;                    //200 //TODO: also enum
                    response.ResponseText = "Path not found";       //"OK"
                    response.ResponseContent = "<html><body>Hello World!</body></html>";
                    response.Headers.Add("Content-Length", response.ResponseContent.Length.ToString());
                    response.Headers.Add("Content-Type", "text/plain"); //application/json 
                }
            }
            catch (Exception ex)
            {
                //if (known error){} //
                Console.WriteLine(ex.ToString());
                response.ResponseCode = 500;
                response.ResponseText = "Internal Server Error";
                response.ResponseContent = "<html><body>Hello World!</body></html>";

                //return;
            }/*
            finally 
            {
                response.Process();
                Console.WriteLine($"    {Thread.CurrentThread.ManagedThreadId}: finished processing"); Console.Out.Flush();
            }*/

            //Verarbeitung und Aufrufen des BLs
            //...unterschiedliche Endpunkte
            //...application spezifische verarbeitung

            //todo wth dict refernce auf controller mappen
            //todo: add different paths
            
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
