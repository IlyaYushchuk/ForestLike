
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ForestLike.ClientServerLogic;

public class Client
{
    TcpClient clientAPI;
    TcpClient client;
    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    
    //TODO replace ServerConnectionEvent on Action<TcpState>
    public event Action<string> ServerConnectionEvent;
    public event Action<string> ServerDataReceiveEvent;

    public Client()
    {
        clientAPI = new TcpClient();
        client = new TcpClient();

        //todo restuct class constructor
        ConnectClientToServer(clientAPI, IPEndPoint.Parse("127.0.0.1:8888"));
        ConnectClientToServer(client, IPEndPoint.Parse("127.0.0.2:8888"));
        StartListiningServerAsync(client, cancellationTokenSource.Token);
    }

    private async void ConnectClientToServer(TcpClient client, IPEndPoint remoteEp)
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
        clientAPI.Close();
        ServerConnectionEvent?.Invoke("Server disconnected");
    }

    private void StartListiningServerAsync(TcpClient client, CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (true)
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream);
                string? data = await reader.ReadLineAsync();
                if (data != null)
                    ServerDataReceiveEvent?.Invoke(data);
             
                //todo cancellation token
                if (cancellationToken.IsCancellationRequested)
                    break;
                await Task.Delay(1000);
            }
        });
    }

    public async Task<string?> GetDataFromServer()
    {   
        var stream = clientAPI.GetStream();
        var reader = new StreamReader(stream);
        return await reader.ReadLineAsync();
    }

    public async Task SendDataToServer(string message)
    {
        var inputStream = clientAPI.GetStream();
        await inputStream.WriteAsync(Encoding.UTF8.GetBytes(message+"\n"));
    }

    public async Task SendCallToServer(string message)
    {
        var inputStream = client.GetStream();
        await inputStream.WriteAsync(Encoding.UTF8.GetBytes(message + "\n"));
    }

    public async Task<string?> GetCallFromServer()
    {
        var stream = client.GetStream();
        var reader = new StreamReader(stream);
        return await reader.ReadLineAsync();
    }
}
