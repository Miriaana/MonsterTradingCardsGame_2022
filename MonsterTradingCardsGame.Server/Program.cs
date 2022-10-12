using System.Net.Sockets;
using System.Net;
/*using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;*/

namespace MonsterTradingCardsGame.Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            TcpListener listener = new TcpListener(IPAddress.Loopback, 8000);
            listener.Start(5);

            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            while (true)
            {
                try
                {
                    var socket = await listener.AcceptTcpClientAsync();
                    using var writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
                    writer.WriteLine("Welcome to myserver!");
                    writer.WriteLine("Please enter your commands...");

                    using var reader = new StreamReader(socket.GetStream());
                    string message;
                    do
                    {
                        message = reader.ReadLine();
                        Console.WriteLine("received: " + message);
                    } while (message != "quit");
                }
                catch (Exception exc)
                {
                    Console.WriteLine("error occurred: " + exc.Message);
                }
            }
        }
    }
}