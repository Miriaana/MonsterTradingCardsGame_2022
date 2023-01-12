using System.Net.Sockets;
using System.Net;
using MTCGame.Server;
using MTCGame.Server.HTTP;
using MTCGame.Server.MTCG;

Console.WriteLine("Simple http-server! http://localhost:10001/");
Console.WriteLine();

var server = new HttpServer(IPAddress.Any, 10001);
server.RegisterEndpoint("users", new UsersEndpoint());
server.RegisterEndpoint("sessions", new SessionsEndpoint());
server.RegisterEndpoint("packages", new PackagesEndpoint());
server.RegisterEndpoint("transactions", new TransactionsEndpoint());
server.RegisterEndpoint("cards", new CardsEndpoint());
server.RegisterEndpoint("deck", new DeckEndpoint());
server.RegisterEndpoint("stats", new StatsEndpoint());
server.RegisterEndpoint("scoreboard", new ScoreboardEndpoint());
server.RegisterEndpoint("battles", new BattlesEndpoint());
server.RegisterEndpoint("tradings", new TradingsEndpoint());

server.run();
//new HttpServer(IPAddress.Any, 10001).run(); //or IPAddress.Loopback
