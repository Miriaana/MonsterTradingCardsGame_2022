using System.Net.Sockets;
/*using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;*/

namespace MTCGame.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Thread.Sleep(2000); // 2 sec so the server is up and running

            using TcpClient client = new TcpClient("localhost", 8000);
            using StreamReader reader = new StreamReader(client.GetStream());
            Console.WriteLine($"{reader.ReadLine()}{Environment.NewLine}{reader.ReadLine()}");
            using StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };

            string input = null;
            while ((input = Console.ReadLine()) != "quit")
            {
                writer.WriteLine(input);
            }
            writer.WriteLine("quit");
        }
    }
}