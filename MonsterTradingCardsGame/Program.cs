// See https://aka.ms/new-console-template for more information

using MonsterTradingCardsGame.BL;
using MonsterTradingCardsGame.DAL;
using MonsterTradingCardsGame.Model;

Console.WriteLine("Hello, World!");
Console.WriteLine("This is just a little test!");

PostgreSQLRepository postgreSQLRepository = new PostgreSQLRepository();
GameHandler game = new GameHandler();

User player1 = new User("Miriam", "1234");
User player2 = new User("Vanessa", "0000");

postgreSQLRepository.CreateUser(player1);

game.PlayBattle(player1, player2);

var newUser = new User("Karl", "1111");
var userHandler = new UserHandler();
userHandler.RegisterUser(newUser);

//Console.WriteLine($"My name is {user1.Username}.");
//user1.Username = "Vanessa";
//Console.WriteLine($"But now I changed it to {user1.Username}.");