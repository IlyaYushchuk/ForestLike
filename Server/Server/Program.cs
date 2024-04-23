// See https://aka.ms/new-console-template for more information


using Server;
using Server.Entities;
using ServerNS;


List<User> us = new List<User>{
    new User {Id = 0, Login = "user1", HashPassword = "12345" },
    new User {Id = 0, Login = "user2", HashPassword = "23456" },
    new User {Id = 0, Login = "user3", HashPassword = "34567" },
    new User {Id = 0, Login = "user4", HashPassword = "45678" },
    new User {Id = 0, Login = "user5", HashPassword = "56789" }
    };

Console.WriteLine(Directory.GetCurrentDirectory());
ApplicationDBContext applicationDBContext = new ApplicationDBContext();
ForestLikeServer server = new ForestLikeServer();

applicationDBContext.Users.AddRange(us);
applicationDBContext.SaveChanges();
server.StartServer();

Console.WriteLine("Input anything to answer YES");
Console.ReadLine();
Console.WriteLine("Sended YES");

await server.SendMessageToUserAsync("Ilya", "/YES");

Console.WriteLine("Input anything to send FAIL");
Console.ReadLine();
Console.WriteLine("Sended FAIL");

await server.SendMessageToUserAsync("Ilya","/FAIL");

Console.ReadLine();
Console.ReadLine();
