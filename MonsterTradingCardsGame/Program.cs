// See https://aka.ms/new-console-template for more information
using MonsterTradingCardsGame;

Console.WriteLine("Hello, World!");
Console.WriteLine("This is just a little test!");

User user1 = new User("Miriam", "1234");
Console.WriteLine($"My name is {user1.Username}.");
user1.Username = "Vanessa";
Console.WriteLine($"But now I changed it to {user1.Username}.");