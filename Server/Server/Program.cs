// See https://aka.ms/new-console-template for more information
using Server = ServerNS.Server;


Server server = new Server();

server.StartServer();
await Task.Delay(10000);
await server.SendMessageToUserAsync("Ilya", "Hello, Ilya\n");

Console.ReadLine();
