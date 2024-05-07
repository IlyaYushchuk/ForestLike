using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ForestLike.ClientServerLogic;
using Serializer.Entities;

namespace ForestLike.Services;

public class CooperativeTimerService
{
    Client client;
    public event Action<string> ServerCommandEvent;
    public CooperativeTimerService(Client client)
    {
        this.client = client;
        //clientAPI.ServerDataReceiveEvent += PrintDataFromServer;
        this.client.ServerDataReceiveEvent += CommandHandler;
    }

    private void CommandHandler(string data)
    {
        string message = data;
        if (message == "/YES")
        {
            ServerCommandEvent?.Invoke("/YES");
           // Console.WriteLine("Another user agree");
        }else if(message == "/NO")
        {
            ServerCommandEvent?.Invoke("/NO");
           // Console.WriteLine("Another user disagree");
        }else if(message == "/FAIL")
        {
            ServerCommandEvent?.Invoke("/FAIL");
           // Console.WriteLine("User failed timer");
        }
    }
    public string GetString(byte[] data)
    {
        return Encoding.UTF8.GetString(data);
    }

    //TODO temporary function
    public void PrintDataFromServer(byte[] data)
    {
        Console.WriteLine($"Server: {Encoding.UTF8.GetString(data)}");
    }
   
    public void CallUserCooperativeTimer(CooperativeTimerRequest cooperativeTimerRequest)
    {
        string serializedRequest = JsonSerializer.Serialize(cooperativeTimerRequest);

        client.SendDataToServer(serializedRequest);
    }
}
