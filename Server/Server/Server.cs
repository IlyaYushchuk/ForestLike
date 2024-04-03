
using System.Net.Sockets;
using System.Net;
using System.Net.Http;
using System.Text;


namespace ServerNS;

public class Server
{
    Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();
    TcpListener server;
    public Server()
    { 
        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        IPEndPoint ipLocalEndPoint = new IPEndPoint(localAddr, 8888);
        server = new TcpListener(ipLocalEndPoint);
    }

    public void StartServer()
    {
        Console.WriteLine("Start");
        server.Start();
        Task.Run(async () =>
        {
            while (true)
            {
                var tcpClient = await server.AcceptTcpClientAsync();

                Console.WriteLine($"Входящее подключение: {tcpClient.Client.RemoteEndPoint}");

                string userName = ListenUserName(tcpClient);
                StartListenUser(userName);
            }
        });
        Console.WriteLine("End");
    }
    public string ListenUserName(TcpClient tcpClient)
    {
        var stream = tcpClient.GetStream();
        
            var response = new List<byte>();
            int bytesRead = 10;

            while ((bytesRead = stream.ReadByte()) != '\n')
            {
                response.Add((byte)bytesRead);
            }
            string name = Encoding.UTF8.GetString(response.ToArray());
            Console.WriteLine($"User name {name}, tcpClient {tcpClient.Client.RemoteEndPoint}");
            clients.Add(name, tcpClient);
            return name;
    }

    public async Task SendMessageToUserAsync(string userName, string message)
    {
        if (!clients.ContainsKey(userName))
        {
            Console.WriteLine("User not found");
            return;
        }

        var stream = clients[userName].GetStream();
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        await stream.WriteAsync(bytes);
    }
    public void StartListenUser(string userName)
    {
        Console.WriteLine("StartListenUserListen");
   
            while (true)
            {
                var stream = clients[userName].GetStream();

                // буфер для входящих данных
                var response = new List<byte>();
                int bytesRead = 10;

                // считываем данные до конечного символа
                while ((bytesRead = stream.ReadByte()) != '\n')
                {
                    // добавляем в буфер
                    response.Add((byte)bytesRead);
                }
                var word = Encoding.UTF8.GetString(response.ToArray());
                Console.WriteLine($"Client {userName} send message {word}");
                if (word == "END\n")
                    break;
                response.Clear();
            }
     
    }
     

}
