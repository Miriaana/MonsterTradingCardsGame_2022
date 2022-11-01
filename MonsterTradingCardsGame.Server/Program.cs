using System.Net.Sockets;
using System.Net;
using MTCGame.Server;
/*using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;*/

Console.WriteLine("Simple http-server! http://localhost:8000/");
Console.WriteLine();

new HttpServer(IPAddress.Any, 10001).run();

/*
namespace MonsterTradingCardsGame.Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, Server up and running!");

            TcpListener listener = new TcpListener(IPAddress.Loopback, 10001);
            listener.Start(5);

            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            while (true)
            {
                try
                {
                    var socket = await listener.AcceptTcpClientAsync();
                    using var writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
                    //writer.WriteLine("Welcome to myserver!");
                    //writer.WriteLine("Please enter your commands...");

                    using var reader = new StreamReader(socket.GetStream());
                    string message;
                    do
                    {
                        message = reader.ReadLine();
                        Console.WriteLine("received: " + message);
                        writer.WriteLine("received: " + message);
                    } while (message != "quit");
                }
                catch (Exception exc)
                {
                    Console.WriteLine("error occurred: " + exc.Message);
                }
            }
        }
    }
}*/