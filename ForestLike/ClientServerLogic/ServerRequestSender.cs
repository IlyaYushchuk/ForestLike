using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serializer.Entities;
using Serializer;
using System.Net.Sockets;
using Amazon.Runtime.Internal;

namespace ForestLike.ClientServerLogic;


//TODO rename class
//function list
//1) get user info + records

public enum CooperativeTimerEventTypes
{
    Agree,
    Disagree,
    Call,
    Fail
}
public class ServerRequestSender
{
    Client client;
    public event Action<CooperativeTimerEventTypes, CooperativeTimerRequest> CooperativeTimerEvent;

    CooperativeTimerRequest currentCooperativeTimerRequest;
    public ServerRequestSender(Client client)
    {
        this.client = client;
        this.client.ServerDataReceiveEvent += ServerMessageHandler;
    }

    public async void ServerMessageHandler(string message)
    {
        if(message.StartsWith("/cooperative request "))
        {
            
            string clearMessage = message.Remove(0, 20);

            currentCooperativeTimerRequest = EntitySerializer.DeserializeCooperativeRequest(clearMessage);
            
            Console.WriteLine($"User {currentCooperativeTimerRequest.SenderName} ask you for cooperative timer");

            Console.WriteLine($"Type {CooperativeTimerEventTypes.Call}, request {EntitySerializer.SerializeCooperativeRequest(currentCooperativeTimerRequest)}");
            CooperativeTimerEvent?.Invoke(CooperativeTimerEventTypes.Call, currentCooperativeTimerRequest);
        }else if(message.StartsWith("/fail"))
        {
            Console.WriteLine("Cooperative request FAIL");
            Console.WriteLine($"Type {CooperativeTimerEventTypes.Fail}, request {EntitySerializer.SerializeCooperativeRequest(currentCooperativeTimerRequest)}");

            CooperativeTimerEvent?.Invoke(CooperativeTimerEventTypes.Fail, currentCooperativeTimerRequest);
        }else if (message.StartsWith("/agree"))
        {
            Console.WriteLine($"Type {CooperativeTimerEventTypes.Agree}, request {EntitySerializer.SerializeCooperativeRequest(currentCooperativeTimerRequest)}");

            CooperativeTimerEvent?.Invoke(CooperativeTimerEventTypes.Agree, currentCooperativeTimerRequest);
        }
        else if (message.StartsWith("/disagree"))
        {
            Console.WriteLine($"Type {CooperativeTimerEventTypes.Disagree}, request {EntitySerializer.SerializeCooperativeRequest(currentCooperativeTimerRequest)}");

            CooperativeTimerEvent?.Invoke(CooperativeTimerEventTypes.Disagree, currentCooperativeTimerRequest);
        }
        else if(message.StartsWith("/fail"))
        {
            Console.WriteLine($"Type {CooperativeTimerEventTypes.Fail}, request {EntitySerializer.SerializeCooperativeRequest(currentCooperativeTimerRequest)}");

            CooperativeTimerEvent?.Invoke(CooperativeTimerEventTypes.Fail, currentCooperativeTimerRequest);
        }
    }


    public async void FailCooperativeTimer()
    {
        Console.WriteLine("You failed timer");
        await client.SendCallToServer("/fail");
    }
    public async void AgreeOnCooperativeRequest()
    {
        Console.WriteLine("You answered AGREE");
        await client.SendCallToServer("/agree");
    }
    public async void DisagreeOnCooperativeRequest()
    {
        Console.WriteLine("You answered DISAGREE");

        await client.SendCallToServer("/disagree");
    }
    public async Task SendCooperativeRequest(CooperativeTimerRequest coopRequest)
    {
        string request = "/cooperative request ";
        request += EntitySerializer.SerializeCooperativeRequest(coopRequest);
        
        //TODO delete
        Console.WriteLine($"You send coop request to {coopRequest.ReceiverName}");

        currentCooperativeTimerRequest = coopRequest;
        await client.SendCallToServer(request);
    }
    public async Task<string> UploadNewRecords(IEnumerable<Record> records)
    {
        string request = "/upload ";
        request += EntitySerializer.SerializeRecords(records);
        await client.SendDataToServer(request);
        string response = await client.GetDataFromServer();

        //TODO deete next string
        Console.WriteLine(response);
        return response;
    }
    public async Task<IEnumerable<Record>> GetRecords()
    {
        string request = "/get records ";
        await client.SendDataToServer(request);
        string response = await client.GetDataFromServer();
        Console.WriteLine(response);
        return EntitySerializer.DeserializeRecords(response);
    }
    public async Task<IEnumerable<string>>GetAllOnlineUsers()
    {
        string request = "/online users ";
        await client.SendDataToServer(request);
        string response = await client.GetDataFromServer();
        Console.WriteLine(response);
        return EntitySerializer.DeserializeNameList(response);
    }
}