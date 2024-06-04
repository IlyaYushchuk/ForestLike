
using ForestLike.Services;
using Serializer;
using Serializer.Entities;
using Server.DBServices;
using ServerNS;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;

namespace Server;


public class ServerCommandHandler
{
    ForestLikeServer server;
    IStorageController dbImmitation;
    
    Dictionary<TcpClient, Tuple<int,string>> clients = new Dictionary<TcpClient, Tuple<int, string>>();
    List<Tuple<TcpClient,TcpClient>> tcpClientsPairs = new List<Tuple<TcpClient, TcpClient>>();
    List<Tuple<TcpClient, TcpClient>> currentConnections = new List<Tuple<TcpClient, TcpClient>>();

    public ServerCommandHandler()
    {
        dbImmitation = new FileDBImmitation();
        server = new ForestLikeServer();
        server.StartServer();
        server.UserMessageEventAPI += UserMessageAuthorizationHandler;
        server.UserMessageEventAPI += UserMessageHandler;
        server.UserMessageEvent += UserMessageCooperativeTimerHandler;

        server.UserConnectionEvent += UserConnection;
    }

    private void UserConnection(TcpClient tcpClientAPI, TcpClient tcpClient)
    {
        Console.WriteLine($"User connected API: {tcpClientAPI.Client.RemoteEndPoint} sighalR: {tcpClient.Client.RemoteEndPoint}");
        tcpClientsPairs.Add(Tuple.Create(tcpClientAPI,tcpClient));
    }




    public async void UserMessageCooperativeTimerHandler(TcpClient tcpClient, string message)
    {
        if (message.StartsWith("/cooperative request "))
        {
            Console.WriteLine("Cooperative request");

            string clearMessage = message.Remove(0, 20);

            //TODO delete
            Console.WriteLine(clearMessage);

            CooperativeTimerRequest cooperativeRequest = EntitySerializer.DeserializeCooperativeRequest(clearMessage);
            Console.WriteLine(tcpClient.Client.RemoteEndPoint);
            Console.WriteLine(GetClientApiByClient(tcpClient).Client.RemoteEndPoint);

            string senderName = clients.FirstOrDefault(x => x.Key == GetClientApiByClient(tcpClient)).Value.Item2;
            cooperativeRequest.SenderName = senderName;

            Console.WriteLine(senderName);
            string ReceiverName = cooperativeRequest.ReceiverName;
            //var smth = ;
            TcpClient receiverClient = GetClientByClientApi(clients.FirstOrDefault(x => x.Value.Item2 == ReceiverName).Key);

            currentConnections.Add(Tuple.Create(tcpClient, receiverClient));

            Console.WriteLine($"Sender {tcpClient.Client.RemoteEndPoint}\n Receiver {receiverClient.Client.RemoteEndPoint}");
            Console.WriteLine(receiverClient);

            string request = "/cooperative request " + EntitySerializer.SerializeCooperativeRequest(cooperativeRequest);

            Console.WriteLine(request);
            await server.SendMessageToUser(receiverClient, request);
        }
        else if(message == "/agree")
        {
            Console.WriteLine("User answer AGREE");

            var connection = currentConnections.FirstOrDefault(con => con.Item2 == tcpClient);
            Console.WriteLine(connection.Item1.Client.RemoteEndPoint);
            Console.WriteLine(connection.Item2.Client.RemoteEndPoint);
            await server.SendMessageToUser(connection.Item1, "/agree");

//            await server.SendMessageToUser(connection.Item2, "/agree");
        }
        else if (message == "/disagree")
        {
            Console.WriteLine("User answer DISAGREE");
            var connection = currentConnections.FirstOrDefault(con => con.Item2 == tcpClient);
            Console.WriteLine(connection.Item1.Client.RemoteEndPoint);
            await server.SendMessageToUser(connection.Item1, "/disagree");
        }else if(message == "/fail")
        {
            Console.WriteLine("User failed timer");
            var connection = currentConnections.FirstOrDefault(con => con.Item1 == tcpClient || con.Item2 == tcpClient);
            Console.WriteLine(connection.Item1.Client.RemoteEndPoint);
            Console.WriteLine(connection.Item2.Client.RemoteEndPoint);
            if(tcpClient == connection.Item1)
                await server.SendMessageToUser(connection.Item2, "/fail");
            else
            await server.SendMessageToUser(connection.Item1, "/fail");

        }
    }

    public async void UserMessageHandler(TcpClient tcpClient, string message)
    {
        if (message.StartsWith("/get records "))
        {
            Console.WriteLine("Records sending");

            List<Record> records = dbImmitation.GetRecordsOfUser(clients[tcpClient].Item1).ToList();
            string serializedResords = EntitySerializer.SerializeRecords(records);
            Console.WriteLine(serializedResords);
            await server.SendMessageToUser(tcpClient, serializedResords);
        }
        else if (message.StartsWith("/upload "))
        {
            Console.WriteLine("Uploading");
            string clearMessage = message.Remove(0, 7);
            Console.WriteLine(clearMessage);
            dbImmitation.UploadNewRecords(clients[tcpClient].Item1,EntitySerializer.DeserializeRecords(clearMessage));
            string response = "";
            try
            {
                var newRecords = EntitySerializer.DeserializeRecords(clearMessage);
                int key = clients[tcpClient].Item1;
                Console.WriteLine($" tcpClient.RemoteEndPoint {tcpClient.Client.RemoteEndPoint}");
                response = "records successfully uploaded";
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                response = "records are broken. we cant save them";
            }
            finally
            {
                await server.SendMessageToUser(tcpClient, response);
            }
                
        }
        else if(message.StartsWith("/online users "))
        {
            Console.WriteLine("Sending all online users");
            
            List<Tuple<int, string>> onlineUsersIdsNames = clients.Values.ToList();
            Console.WriteLine("Online users:");
            foreach (Tuple<int, string> userId in onlineUsersIdsNames) 
            {
                Console.WriteLine(userId.Item2);
            }
            List<string> onlineUsersNames = onlineUsersIdsNames.Select(u => u.Item2).ToList();
            
            string response = EntitySerializer.SerializeNameList(onlineUsersNames);
            Console.WriteLine(response);
            await server.SendMessageToUser(tcpClient, response);

        }
    }
    public async void UserMessageAuthorizationHandler(TcpClient tcpClient, string message)
    {

        if(message.StartsWith("/registration "))
        {

            Console.WriteLine("Registration");
            string clearMessage = message.Remove(0, 13);
            Console.WriteLine(clearMessage);
            var user = EntitySerializer.DeserializeUser(clearMessage);
            var searchUser = dbImmitation.GetUserInfo(user);
            string response;
            if (searchUser == null || (user.Login != searchUser.Login && user.HashPassword != searchUser.HashPassword))
            {
                int userId = dbImmitation.UploadNewUser(user);
                clients[tcpClient] = Tuple.Create(userId, user.Login);
                response = "user successfully registered";
            }
            else
            {
               response = "such user already exist";
            }

            Console.WriteLine(response);
            await server.SendMessageToUser(tcpClient, response);
        }
        else if(message.StartsWith("/login "))
        {
            Console.WriteLine("Login");
            string clearMessage = message.Remove(0, 6);
            Console.WriteLine(clearMessage);
            User user = EntitySerializer.DeserializeUser(clearMessage);
            User userInfo = dbImmitation.GetUserInfo(user);
            Console.WriteLine(EntitySerializer.SerializeUser(userInfo));
            string response;
            if (userInfo != null)
            {
                response = "user successfully login";
                clients[tcpClient] = Tuple.Create(userInfo.Id, user.Login);
            }
            else
            {
                response = "no such user";
            }
            Console.WriteLine($"{tcpClient.Client.RemoteEndPoint} {response}");
            await server.SendMessageToUser(tcpClient, response);
        }else if (message.StartsWith("/logout "))
        {
            Console.WriteLine("Logout");
            clients.Remove(tcpClient);
            string response = "successfully logout";
            await server.SendMessageToUser(tcpClient, response);
        }
    }
    private TcpClient GetClientApiByClient(TcpClient tcpClient)
    {
        return tcpClientsPairs.FirstOrDefault(tcpPair => tcpPair.Item2 == tcpClient).Item1;
    }
    private TcpClient GetClientByClientApi(TcpClient tcpClientAPI)
    {
        return tcpClientsPairs.FirstOrDefault(tcpPair => tcpPair.Item1 == tcpClientAPI).Item2;
    }
}
