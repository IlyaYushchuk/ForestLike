
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ForestLike.ClientServerLogic;

public class Client
{
    TcpClient client;
    public Client()
    {
        client = new TcpClient();
    }

    public event Action<string> ServerConnectionEvent;
    public event Action<string> ServerEvent;

    public async void ConnectClientToServer(IPEndPoint remoteEp)
    {
        await client.ConnectAsync(remoteEp);
        if (client.Connected)
            ServerConnectionEvent?.Invoke("Server connected");
        else
            ServerConnectionEvent?.Invoke("Server wan't connected");
    }

    public void StartListininServerAsync()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                var stream = client.GetStream();
                var response = new List<byte>();
                int bytesRead = 10;
                while ((bytesRead = stream.ReadByte()) != '\n')
                {
                    response.Add((byte)bytesRead);
                }
                var translation = Encoding.UTF8.GetString(response.ToArray());
                Console.WriteLine($"Server: {translation}");
                response.Clear();
                //todo cancellation token
                if (translation == "END\n")
                    break;
                await Task.Delay(1000);
            }
        });
    }

    public async void SendMessage(string str)
    {
        NetworkStream inputStream = client.GetStream();
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        await inputStream.WriteAsync(bytes);
    }
}
