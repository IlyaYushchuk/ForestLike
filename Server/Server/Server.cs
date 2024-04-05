
using System.Net.Sockets;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;


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

    public IEnumerable<string> GetAllClientsNames()
    {
        return clients.Keys;
    }

    private void MessageResending(CooperativeTimerRequest request)
    {
        SendMessageToUserAsync(request.UserName, JsonSerializer.Serialize(request));
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
                
                string userName = ListenUserName(tcpClient).Result;
                StartListenUser(userName);
            }
        });
        Console.WriteLine("End");
    }
    public async Task<string> ListenUserName(TcpClient tcpClient)
    {
        var stream = tcpClient.GetStream();
        var reader = new StreamReader(stream);
        string name = await reader.ReadLineAsync();
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
        byte[] bytes = Encoding.UTF8.GetBytes(message + "\n");
        await stream.WriteAsync(bytes);
    }
    public async Task StartListenUser(string userName)
    {
            while (true)
            {
                var stream = clients[userName].GetStream();
                var reader = new StreamReader(stream);
                string message = await reader.ReadLineAsync();
                CooperativeTimerRequest request = JsonSerializer.Deserialize<CooperativeTimerRequest>(message);

                MessageResending(request);
               // Console.WriteLine($"Client {userName} send message {message}");
                Console.WriteLine($"User {userName}, message {request.Description}");        
                if (message == "END\n")
                    break;
            }
     
    }
     

}
