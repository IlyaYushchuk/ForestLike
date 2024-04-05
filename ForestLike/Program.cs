using ForestLike;
using ForestLike.Entities;
using ForestLike.TimeCounters;
using ForestLike.ClientServerLogic;
using ForestLike.Services;
using System.Security.Authentication;
using System.Text.Json;

using Timer = ForestLike.TimeCounters.Timer;
using SQLite;
using System.Diagnostics;





Console.WriteLine("Input number of use-case");
string selectedCase = Console.ReadLine();

 ITimeCounter TimeCounter = new Timer();
 User? User = new User();
IStorageController StorageController = new LocaleStorageController();
AuthorizationService AuthorizationService = new AuthorizationService(StorageController);

 List<Record> Records;

/*
User = AuthorizationService.Registration("User1", "12345");

List<Record> records = new List<Record>();
records.Add(new Record
{
    Time = new TimeSpan(1, 0, 0),
    RecordId = 1,
    UserId = User.Id,
    Theme = "Work",
    StartDate = DateTime.Now,
    IsFailed = false,
    FailedTime = new TimeSpan(0, 0, 0)
});
records.Add(new Record
{
    Time = new TimeSpan(2, 30, 0),
    RecordId = 1,
    UserId = User.Id,
    Theme = "Rest",
    StartDate = new DateTime(2024, 02, 28),
    IsFailed = false,
    FailedTime = new TimeSpan(0, 0, 0)
});
StorageController.UploadNewRecords(User.Id, records);

*/

int k = 6;

switch (selectedCase)
{
    case ("1"):
        Console.WriteLine("1 Registration example");
        User = AuthorizationService.Registration("User2", "123456");
        Console.WriteLine($"User: login {User.Login}, hashed password {User.HashPassword}, Id {User.Id}");
        break;
    case ("2"):
        Console.WriteLine("2 Login example");
      
        User = AuthorizationService.Login("User1", "12345");
        Records = StorageController.GetRecordsOfUser(User.Id).ToList();
        foreach (Record record in Records)
        {
            Console.WriteLine($"Record: " +
                $"type {record.Type} " +
                $"theme {record.Theme}, " +
                $"time {record.Time}, " +
                $"start date {record.StartDate}, " + 
                $"was it failed {record.IsFailed}, " +
                $"failed time {record.FailedTime}" );
        }
        break;
    case ("3"):
        
        Console.WriteLine("3 Timer use example");
        Timer timer = TimeCounter as Timer;
        timer.TimerTick += async (currTime) =>
        {
            Console.SetCursorPosition(0, 3);
            Console.Write($"\r{currTime}");
        };
        timer.Notification += async (str) =>
        {
            await Task.Run(() =>
            {
                Task.Delay(17);
                Console.SetCursorPosition(0, k++);
                Console.Write(str);
            });
        };

        timer.SetTheme("Example 3");
        timer.SetTime(new TimeSpan(0,0,30));

        timer.SetHardMode();

        timer.StartTime();
        await Task.Delay(10000);
        timer.StopTime();
        Record record3 = timer.GetRecord();
        Console.WriteLine($"Record: " +
             $"type {record3.Type} " +
             $"theme {record3.Theme}, " +
             $"time {record3.Time}, " +
             $"start date {record3.StartDate}, " +
             $"was it failed {record3.IsFailed}, " +
             $"failed time {record3.FailedTime}");
        break;

    case ("4"):
        Console.WriteLine("4 StopWatch EasyMode example");
        StopWatch sw = new StopWatch();
        sw.TimerTick += async (currTime) =>
        {
            Console.SetCursorPosition(0, 3);
            Console.Write($"\r{currTime}");
        };
        sw.Notification += async (str) =>
        {
            await Task.Run(() =>
            {
                Task.Delay(17);
                Console.SetCursorPosition(0, k++);
                Console.Write(str);
            });
        };

        sw.SetTheme("Example 4 Stopwatch");

        sw.SetEasyMode();

        sw.StartTime();
        await Task.Delay(12000);
        sw.StopTime();
        Record record4 = sw.GetRecord();
        Console.WriteLine($"Record: " +
             $"type {record4.Type} " +
             $"theme {record4.Theme}, " +
             $"time {record4.Time}, " +
             $"start date {record4.StartDate}, " +
             $"was it failed {record4.IsFailed}, " +
             $"failed time {record4.FailedTime}");
        break;

       case ("5"):
        Console.WriteLine("5 Cooperative timer EasyMode example");
        CooperativeTimer ct = new CooperativeTimer("Ilya");
        ct.TimerTick += async (currTime) =>
        {
            Console.SetCursorPosition(0, 3);
            Console.Write($"\r{currTime}");
        };
        ct.Notification += async (str) =>
        {
            await Task.Run(() =>
            {
                Task.Delay(17);
                Console.SetCursorPosition(0, k++);
                Console.Write(str);
            });
        };

        ct.SetTheme("Example 5 CooperativeTimer");

        ct.SetEasyMode();

        ct.StartTime("Vlad","Please agree");
        //await Task.Delay(12000);
        //ct.StopTime();
        Console.ReadLine();
        Record record5 = ct.GetRecord();
        Console.WriteLine($"Record: " +
             $"type {record5.Type} " +
             $"theme {record5.Theme}, " +
             $"time {record5.Time}, " +
             $"start date {record5.StartDate}, " +
             $"was it failed {record5.IsFailed}, " +
             $"failed time {record5.FailedTime}");
        break;

    case ("6"):

        Console.WriteLine("6 Timer HardMode plus cats use example");
        Timer timer6 = new Timer();
        timer6.TimerTick += async (currTime) =>
        {
            Console.SetCursorPosition(0, 3);
            Console.Write($"\r{currTime}");
        };
        timer6.Notification += async (str) =>
        {
            await Task.Run(() =>
            {
                Task.Delay(17);
                Console.SetCursorPosition(0, k++);
                Console.Write(str);
            });
        };

        List<Process> processlist = ActivityObserver.GetInstance().GetAllOpenedWindows().ToList();

        //foreach (Process process in processlist)
        //{
        //    if (!String.IsNullOrEmpty(process.MainWindowTitle))
        //    {
        //        Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
        //    }
        //}

        Process catsMemes = processlist.Where(x => x.MainWindowTitle == "Мемы про котов - Поиск в Google - Google Chrome").ToList()[0];

        ActivityObserver.GetInstance().AddAllowedApp(catsMemes);

        timer6.SetTheme("Example 6");
        timer6.SetTime(new TimeSpan(0, 1, 30));

        timer6.SetHardMode();

        timer6.StartTime();
        await Task.Delay(new TimeSpan(0,1,32));
        //timer6.StopTime();
        Record record6 = timer6.GetRecord();
        Console.WriteLine($"Record: " +
             $"type {record6.Type} " +
             $"theme {record6.Theme}, " +
             $"time {record6.Time}, " +
             $"start date {record6.StartDate}, " +
             $"was it failed {record6.IsFailed}, " +
             $"failed time {record6.FailedTime}");
        break;
}







/*
//Client client = new Client();

//client.ConnectClient();
//client.StartListininServerAsync();
//client.SendMessage("Ilya\n");
//client.SendMessage("Test message1!\n");
//client.SendMessage("Test message2!\n");

//client.SendMessage("END!\n");



//LocaleStorageController localeStorageController = new LocaleStorageController();
//Console.WriteLine("Input your name");
//string name = Console.ReadLine();

//while (true)
//{

//    CooperativeTimerService cooperativeTimerService = new CooperativeTimerService(name);
//    CooperativeTimerRequest request = new CooperativeTimerRequest();
//    Console.WriteLine("Input to who you want to write");
//    request.UserName = Console.ReadLine();
//    Console.WriteLine("Input description");
//    request.Description = Console.ReadLine();
//    request.Theme = "Work";
//    request.Time = new TimeSpan(1, 0, 0);

//    cooperativeTimerService.CallUserCooperativeTimer(request);
//    Console.ReadLine();
//}
//CooperativeTimer mt = new CooperativeTimer("Ilya");



//int i = 0, j = 0;

//mt.TimerTick += async (currTime) =>
//{
//    Console.SetCursorPosition(0, 2);
//    Console.Write($"\r{currTime}");
//};
//int k = 6;
//mt.Notification += async (str) =>
//{
//    await Task.Run(() =>
//    {
//        Task.Delay(17);
//        Console.SetCursorPosition(0, k++);
//        Console.Write(str);
//    }
//    );

//};


//mt.SetTime(new TimeSpan(0, 1, 00));

//Console.ReadLine();
//mt.SetHardMode();
//mt.SetTheme("enjoing coffee!");

//mt.StartTime("Ilya2", "Test");

//Console.WriteLine();
//Console.WriteLine("1) Stop\n\n");
//string something = Console.ReadLine();

//Console.WriteLine();
//Console.WriteLine($"something {something}");
//if (something == "1")
//{
//    mt.StopTime();
//}

//Record rec = mt.GetRecord();

//Console.WriteLine($"Record {JsonSerializer.Serialize(rec)}");
//Console.ReadLine();
//Console.ReadLine();*/