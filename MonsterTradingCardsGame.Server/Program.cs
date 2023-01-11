using System.Net.Sockets;
using System.Net;
using MTCGame.Server;
using MTCGame.Server.HTTP;
using MTCGame.Server.MTCG;

Console.WriteLine("Simple http-server! http://localhost:10001/");
Console.WriteLine();

var server = new HttpServer(IPAddress.Any, 10001);
server.RegisterEndpoint("users", new UsersEndpoint());
server.run();
//new HttpServer(IPAddress.Any, 10001).run(); //or IPAddress.Loopback
