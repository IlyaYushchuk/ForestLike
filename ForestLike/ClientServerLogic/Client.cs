
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ForestLike.ClientServerLogic;

public class Client
{
    TcpClient client;
    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    //TODO replace ServerConnectionEvent on Action<TcpState>
    public event Action<string> ServerConnectionEvent;
    public event Action<byte[]> ServerDataReceiveEvent;

    public Client(string name)
    {
        client = new TcpClient();
        //todo restuct class constructor
        
        ConnectClientToServer(IPEndPoint.Parse("127.0.0.1:8888"));
       
        SendToServerName(name);
        StartListininServerAsync(cancellationTokenSource.Token);
    }

    public void SendToServerName(string name)
    {
        SendDataToServer(Encoding.UTF8.GetBytes(name + "\n"));
    }
    public async void ConnectClientToServer(IPEndPoint remoteEp)
    {
        try
        {
            await client.ConnectAsync(remoteEp);
            ServerConnectionEvent?.Invoke("Server connected");
        }
        catch (Exception ex) 
        {
            ServerConnectionEvent?.Invoke("Server wan't connected. Try again");
            Console.WriteLine(ex.ToString());
        }
    }
    public void DisconnectClientToServer()
    {
        cancellationTokenSource.Cancel();
        client.Close();
        ServerConnectionEvent?.Invoke("Server disconnected");
    }

    public void StartListininServerAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (true)
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream);
                var data = await reader.ReadLineAsync();
                ServerDataReceiveEvent?.Invoke(Encoding.UTF8.GetBytes(data));
             
                //todo cancellation token
                if (cancellationToken.IsCancellationRequested)
                    break;
                await Task.Delay(1000);
            }
        });
    }

    public async void SendDataToServer(byte[] data)
    {
        var inputStream = client.GetStream();
        await inputStream.WriteAsync(data);
    }
}
