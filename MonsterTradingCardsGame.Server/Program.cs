using System.Net.Sockets;
using System.Net;
using MTCGame.Server;
using MTCGame.Server.HTTP;

Console.WriteLine("Simple http-server! http://localhost:10001/");
Console.WriteLine();

new HttpServer(IPAddress.Any, 10001).run(); //or IPAddress.Loopback
