// See https://aka.ms/new-console-template for more information
using Serializer.Entities;
using System.Net.Sockets;
using System.Reflection;


Console.WriteLine(Directory.GetCurrentDirectory());

Assembly asm = Assembly.LoadFrom("D:\\253505\\4sem\\oop\\ForestLike\\ForestLike\\bin\\Debug\\net8.0\\ForestLike.dll");

Console.WriteLine(asm.FullName);

// получаем все типы из сборки MyApp.dll



Type? typeClient = asm.GetType("ForestLike.ClientServerLogic.Client");
Type? typeServerRequestSender = asm.GetType("ForestLike.ClientServerLogic.ServerRequestSender");
Type? typeAuthorizationService = asm.GetType("ForestLike.Services.AuthorizationService");
Type? typeActivityObserver = asm.GetType("ForestLike.ActivityObserver");
Type? typeTimer = asm.GetType("ForestLike.TimeCounters.Timer");

var client = Activator.CreateInstance(typeClient);
//var client2 = Activator.CreateInstance(typeClient);

//ConstructorInfo? constructorSRS = typeServerRequestSender?.GetConstructor(new[] { typeof(TcpClient)});
var serverRequestSender = Activator.CreateInstance(typeServerRequestSender, new object[] { client });
//var serverRequestSender2 = Activator.CreateInstance(typeServerRequestSender, new object[] { client2 });

//ConstructorInfo constructorAS = typeAuthorizationService.GetConstructor(new[] { typeof(TcpClient) });
var authorizService = Activator.CreateInstance(typeAuthorizationService, new object[] { client });
//var authorizService2 = Activator.CreateInstance(typeAuthorizationService, new object[] { client2 });



// Вызов метода
MethodInfo Register = typeAuthorizationService.GetMethod("Register", new[] { typeof(string), typeof(string) });
MethodInfo Login = typeAuthorizationService.GetMethod("Login", new[] { typeof(string), typeof(string) });
MethodInfo SendCooperativeRequest = typeServerRequestSender.GetMethod("SendCooperativeRequest", new[] { typeof(CooperativeTimerRequest)});



bool repeat = true;


while (repeat)
{

    Console.WriteLine("Input number of use-case");
    string selectedCase = Console.ReadLine();



    int k = 6;
    string login = "", password = "";
    switch (selectedCase)
    {
        case ("1"):
            Console.WriteLine("1 Register example");
            Console.WriteLine("Input login");
            login = Console.ReadLine();
            Console.WriteLine("Input password");
            password = Console.ReadLine();
            Register.Invoke(authorizService, new object[] { login, password });
            break;

        case ("2"):
            Console.WriteLine("2 Login example");
            Console.WriteLine("Input login");
            login = Console.ReadLine();
            Console.WriteLine("Input password");
            password = Console.ReadLine();
            Login.Invoke(authorizService, new object[] { login, password });
            break;

        case ("3"):
            Console.WriteLine("3 Records get example");
            Login.Invoke(authorizService, new object[] { "Genry3", "password" });
            MethodInfo? GetRecords = typeServerRequestSender.GetMethod("GetRecords");
            object result = GetRecords.Invoke(serverRequestSender, null);

            IEnumerable<Record> records = result as IEnumerable<Record>;
            //foreach (Record record in records.ToList())
            //{
            //    Console.WriteLine($"Record: " +
            //        $"type {record.Type} " +
            //        $"theme {record.Theme}, " +
            //        $"time {record.Time}, " +
            //        $"start date {record.StartDate}, " +
            //        $"was it failed {record.IsFailed}, " +
            //        $"failed time {record.FailedTime}");
            //}
            break;

        case ("4"):
            Console.WriteLine("4 Upload new record ");
            Console.WriteLine("Enter theme of record");
            string theme = Console.ReadLine();

            List<Record> test4Records = new List<Record> { 
                new Record { Theme = theme, Time = new TimeSpan(1, 30, 0), FailedTime = new TimeSpan(1, 29, 59), IsFailed = true, StartDate = DateTime.Now, Type = TimeCounterType.Timer }
            };
            Login.Invoke(authorizService, new object[] { "Steven", "3425" });

            MethodInfo? UploadNewRecords = typeServerRequestSender.GetMethod("UploadNewRecords", new[] { typeof(IEnumerable<Record>) });

            UploadNewRecords.Invoke(serverRequestSender, new object[] { test4Records});

           
            break;

        case ("5"):
            Console.WriteLine("5 Cooperative request sender example");

            Console.WriteLine("Login sender user:");
            Login.Invoke(authorizService, new object[] { "Genry3", "password" });
            await Task.Delay(500);
            Console.WriteLine("Do you wnat to send request");
            Console.ReadLine();
            Console.WriteLine("Send request:");
            CooperativeTimerRequest cooperativeTimerRequest = new CooperativeTimerRequest { Description = "Test request", ReceiverName = "Steven", Theme = "Test-Theme" };
            SendCooperativeRequest.Invoke(serverRequestSender, new object[] { cooperativeTimerRequest });
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            break;
        case ("6"):
            Console.WriteLine("6 Cooperative request receiver example");
           
            Console.WriteLine("Login receiver user:");
            Login.Invoke(authorizService, new object[] { "Steven", "3425" });

            await Task.Delay(2000);

            MethodInfo? AgreeOnCooperativeRequest = typeServerRequestSender.GetMethod("AgreeOnCooperativeRequest");


            MethodInfo? DisagreeOnCooperativeRequest = typeServerRequestSender.GetMethod("DisagreeOnCooperativeRequest");


            Console.WriteLine("Do you want to agree on request");
            string answer6 = Console.ReadLine();


            if(answer6 =="YES")
            {
                AgreeOnCooperativeRequest.Invoke(serverRequestSender, null);
            }
            else if (answer6 == "NO")
            {
                DisagreeOnCooperativeRequest.Invoke(serverRequestSender, null);
            }

            
            Console.ReadLine();
            break;
    }
    await Task.Delay(1000);
    Console.WriteLine("Repeat?(yes/no)");
    string answer = Console.ReadLine();
    if (answer == "yes")
    {
        repeat = true;
        Console.Clear();
    }
    else if (answer == "no")
    {
        repeat = false;
    }

}


/*
Client clientAPI = new Client();

clientAPI.ConnectClient();
clientAPI.StartListininServerAsync();
clientAPI.SendMessage("Ilya\n");
clientAPI.SendMessage("Test message1!\n");
clientAPI.SendMessage("Test message2!\n");

clientAPI.SendMessage("END!\n");



LocaleStorageController localeStorageController = new LocaleStorageController();
Console.WriteLine("Input your name");
string name = Console.ReadLine();

while (true)
{

    CooperativeTimerService cooperativeTimerService = new CooperativeTimerService(name);
    CooperativeTimerRequest request = new CooperativeTimerRequest();
    Console.WriteLine("Input to who you want to write");
    request.UserName = Console.ReadLine();
    Console.WriteLine("Input description");
    request.Description = Console.ReadLine();
    request.Theme = "Work";
    request.Time = new TimeSpan(1, 0, 0);

    cooperativeTimerService.CallUserCooperativeTimer(request);
    Console.ReadLine();
}
CooperativeTimer mt = new CooperativeTimer("Ilya");



int i = 0, j = 0;

mt.TimerTick += async (currTime) =>
{
    Console.SetCursorPosition(0, 2);
    Console.Write($"\r{currTime}");
};
int k = 6;
mt.Notification += async (str) =>
{
    await Task.Run(() =>
    {
        Task.Delay(17);
        Console.SetCursorPosition(0, k++);
        Console.Write(str);
    }
    );

};


mt.SetTime(new TimeSpan(0, 1, 00));

Console.ReadLine();
mt.SetHardMode();
mt.SetTheme("enjoing coffee!");

mt.StartTime("Ilya2", "Test");

Console.WriteLine();
Console.WriteLine("1) Stop\n\n");
string something = Console.ReadLine();

Console.WriteLine();
Console.WriteLine($"something {something}");
if (something == "1")
{
    mt.StopTime();
}

Record rec = mt.GetRecord();

Console.WriteLine($"Record {JsonSerializer.Serialize(rec)}");
Console.ReadLine();
Console.ReadLine();*/


Console.ReadLine();