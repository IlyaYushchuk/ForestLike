// See https://aka.ms/new-console-template for more information
using Server = ServerNS.Server;


Server server = new Server();

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
