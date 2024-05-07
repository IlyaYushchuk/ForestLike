using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ServerNS;

public class ForestLikeServer
{
    TcpListener serverAPI;
    TcpListener server;
    public event Action<TcpClient,string> UserMessageEventAPI;
    public event Action<TcpClient, string> UserMessageEvent;
    public event Action<TcpClient, TcpClient> UserConnectionEvent;

    public ForestLikeServer()
    { 
        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        IPEndPoint ipLocalEndPointAPI = new IPEndPoint(localAddr, 8888);
        IPEndPoint ipLocalEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.2"), 8888);

        serverAPI = new TcpListener(ipLocalEndPointAPI);
        server = new TcpListener(ipLocalEndPoint);
    }
    public void StartServer()
    {
        serverAPI.Start();
        server.Start();
        Task.Run(async () =>
        {
            while (true)
            {
                var tcpClient = await server.AcceptTcpClientAsync();
                var tcpClientAPI = await serverAPI.AcceptTcpClientAsync();
                
                Console.WriteLine($"API: {tcpClientAPI.Client.RemoteEndPoint}");

                Console.WriteLine($"Client: {tcpClient.Client.RemoteEndPoint}");

                UserConnectionEvent?.Invoke(tcpClientAPI, tcpClient);

                Task.Run( async ()=> StartListenUser(tcpClient));
                Task.Run(async () => StartListenUserAPI(tcpClientAPI));
            }
        });
    }
    public async Task StartListenUser(TcpClient tcpClient)
    {
        var stream = tcpClient.GetStream();
        var reader = new StreamReader(stream);
        string message;

        while (true)
        {
            message = await reader.ReadLineAsync();
            UserMessageEvent?.Invoke(tcpClient, message);

           // Console.WriteLine($"User {tcpClient.Client.RemoteEndPoint}, message |{message}|");
            if (message == "END")
            {
                Console.WriteLine("Ending");
                break;
            }
        }

    }
    public async Task StartListenUserAPI(TcpClient tcpClient)
    {
        var stream = tcpClient.GetStream();
        var reader = new StreamReader(stream);
        string message;

        while (true)
        {
            message = await reader.ReadLineAsync();
            UserMessageEventAPI?.Invoke(tcpClient, message);

            // Console.WriteLine($"User {tcpClient.Client.RemoteEndPoint}, message |{message}|");
            if (message == "END")
            {
                Console.WriteLine("Ending");
                break;
            }
        }

    }
    public async Task<string?> GetMessageFromUser(TcpClient tcpClient)
    {
        var stream = tcpClient.GetStream();
        var reader = new StreamReader(stream);
        return await reader.ReadLineAsync();
    }

    public async Task SendMessageToUser(TcpClient client, string message)
    {
        var stream = client.GetStream();
        byte[] bytes = Encoding.UTF8.GetBytes(message + "\n");
        await stream.WriteAsync(bytes);
    }

}
