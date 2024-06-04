//using ForestLike;
//using Serializer.Entities;
//using ForestLike.TimeCounters;
//using ForestLike.ClientServerLogic;
//using ForestLike.Services;
//using System.Security.Authentication;
//using System.Text.Json;
//using Serializer;
//using Timer = ForestLike.TimeCounters.Timer;
//using SQLite;
//using System.Diagnostics;
//using Microsoft.AspNetCore.Cryptography.KeyDerivation;
//using System.Security.Cryptography;

//void Handler(CooperativeTimerEventTypes type, CooperativeTimerRequest request)
//{
//    Console.WriteLine($"Type {type}, request {EntitySerializer.SerializeCooperativeRequest(request)}");
//}


//Client client = new Client();
//ServerRequestSender serverRequestSender = new ServerRequestSender(client);
//serverRequestSender.CooperativeTimerEvent += Handler;
//AuthorizationService aService = new AuthorizationService(client);

//User genry = new User { Login = "Genry", HashPassword = "password" };
//User genry2 = new User { Login = "Genry2", HashPassword = "password" };
//User genry3 = new User { Login = "Genry3", HashPassword = "password" };


//Console.WriteLine(EntitySerializer.SerializeToXmlString(genry));

//User serUser = EntitySerializer.DeserializeFromXmlString<User>(EntitySerializer.SerializeToXmlString(genry));
//Console.WriteLine($"Login {serUser.Login}, password {serUser.HashPassword}");


//List<Record> records = new List<Record>();
//records.Add(new Record
//{
//    Time = new TimeSpan(1, 0, 0),
//    RecordId = 1,
//    UserId = genry3.Id,
//    Theme = "Test record-1",
//    StartDate = DateTime.Now,
//    IsFailed = false,
//    FailedTime = new TimeSpan(0, 0, 0)
//});
//records.Add(new Record
//{
//    Time = new TimeSpan(2, 30, 0),
//    RecordId = 1,
//    UserId = genry3.Id,
//    Theme = "Test record-2",
//    StartDate = new DateTime(2024, 02, 28),
//    IsFailed = false,
//    FailedTime = new TimeSpan(0, 0, 0)
//});

//Console.WriteLine("Input your name");
//string name = Console.ReadLine();

//await aService.Register(name, "password");
//Console.WriteLine("Ended registration");


//Console.ReadLine();
//await serverRequestSender.GetAllOnlineUsers();


//Console.WriteLine("Enter name of user to who you want to sent coop request");
//string recName = Console.ReadLine();


//CooperativeTimerRequest ctr = new CooperativeTimerRequest { ReceiverName = recName, Description = "Hello", Theme = "test1", Time = new TimeSpan(0, 30, 0) };
//if (recName != "NO")
//    await serverRequestSender.SendCooperativeRequest(ctr);

//Console.WriteLine("You got cooperative timer rquest YES/NO");
//string yn = Console.ReadLine();

//if (yn == "YES")
//{
//    serverRequestSender.AgreeOnCooperativeRequest();
//}
//else if (yn == "NO")
//{
//    serverRequestSender.DisagreeOnCooperativeRequest();
//}

//Console.ReadLine();
//Console.ReadLine();
//Console.ReadLine();

//bool repeat = true;
////while (repeat)
////{
////    Console.Write("Enter a password: ");
////    string? password = Console.ReadLine();

////    // Generate a 128-bit salt using a sequence of
////    // cryptographically strong random bytes.
////    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
////    Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

////    // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
////    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
////        password: password!,
////        salt: salt,
////        prf: KeyDerivationPrf.HMACSHA256,
////        iterationCount: 100000,
////        numBytesRequested: 256 / 8));

////    Console.WriteLine($"Hashed: {hashed}");
////}




//while (repeat)
//{

//    Console.WriteLine("Input number of use-case");
//    string selectedCase = Console.ReadLine();

//    ITimeCounter TimeCounter = new Timer();
//    User? User = new User();
//    AuthorizationService AuthorizationService = new AuthorizationService(StorageController);

//    List<Record> Records;

//    /*
//    User = AuthorizationService.Register("User1", "12345");

//    List<Record> records = new List<Record>();
//    records.Add(new Record
//    {
//        Time = new TimeSpan(1, 0, 0),
//        RecordId = 1,
//        UserId = User.Id,
//        Theme = "Work",
//        StartDate = DateTime.Now,
//        IsFailed = false,
//        FailedTime = new TimeSpan(0, 0, 0)
//    });
//    records.Add(new Record
//    {
//        Time = new TimeSpan(2, 30, 0),
//        RecordId = 1,
//        UserId = User.Id,
//        Theme = "Rest",
//        StartDate = new DateTime(2024, 02, 28),
//        IsFailed = false,
//        FailedTime = new TimeSpan(0, 0, 0)
//    });
//    StorageController.UploadNewRecords(User.Id, records);

//    */

//    int k = 6;

//    switch (selectedCase)
//    {
//        case ("1"):
//            Console.WriteLine("1 Register example");
//            User = AuthorizationService.Register("User2", "123456");
//            Console.WriteLine($"User: login {User.Login}, hashed password {User.HashPassword}, Id {User.Id}");
//            break;
//        case ("2"):
//            Console.WriteLine("2 Login example");

//            User = AuthorizationService.Login("User1", "12345");
//            Records = StorageController.GetRecordsOfUser(User.Id).ToList();
//            foreach (Record record in Records)
//            {
//                Console.WriteLine($"Record: " +
//                    $"type {record.Type} " +
//                    $"Theme {record.Theme}, " +
//                    $"Time {record.Time}, " +
//                    $"start date {record.StartDate}, " +
//                    $"was it failed {record.IsFailed}, " +
//                    $"failed Time {record.FailedTime}");
//            }
//            break;
//        case ("3"):

//            Console.WriteLine("3 Timer use example");
//            Timer timer = TimeCounter as Timer;
//            timer.TimerTick += async (currTime) =>
//            {
//                Console.SetCursorPosition(0, 3);
//                Console.Write($"\r{currTime}");
//            };
//            timer.Notification += async (str) =>
//            {
//                await Task.Run(() =>
//                {
//                    Task.Delay(17);
//                    Console.SetCursorPosition(0, k++);
//                    Console.Write(str);
//                });
//            };

//            timer.SetTheme("Example 3");
//            timer.SetTime(new TimeSpan(0, 0, 30));

//            timer.SetHardMode();

//            timer.StartTime();
//            await Task.Delay(10000);
//            timer.StopTime();
//            Record record3 = timer.GetRecord();
//            Console.WriteLine($"Record: " +
//                 $"type {record3.Type} " +
//                 $"Theme {record3.Theme}, " +
//                 $"Time {record3.Time}, " +
//                 $"start date {record3.StartDate}, " +
//                 $"was it failed {record3.IsFailed}, " +
//                 $"failed Time {record3.FailedTime}");

//            List<Record> records = new List<Record>();
//            records.Add(record3);

//            StorageController.UploadNewRecords(User.Id, records);

//            break;

//        case ("4"):
//            Console.WriteLine("4 StopWatch EasyMode example");
//            StopWatch sw = new StopWatch();
//            sw.TimerTick += async (currTime) =>
//            {
//                Console.SetCursorPosition(0, 3);
//                Console.Write($"\r{currTime}");
//            };
//            sw.Notification += async (str) =>
//            {
//                await Task.Run(() =>
//                {
//                    Task.Delay(17);
//                    Console.SetCursorPosition(0, k++);
//                    Console.Write(str);
//                });
//            };

//            sw.SetTheme("Example 4 Stopwatch");

//            sw.SetEasyMode();

//            sw.StartTime();
//            await Task.Delay(12000);
//            sw.StopTime();
//            Record record4 = sw.GetRecord();
//            Console.WriteLine($"Record: " +
//                 $"type {record4.Type} " +
//                 $"Theme {record4.Theme}, " +
//                 $"Time {record4.Time}, " +
//                 $"start date {record4.StartDate}, " +
//                 $"was it failed {record4.IsFailed}, " +
//                 $"failed Time {record4.FailedTime}");
//            break;

//        case ("5"):
//            Console.WriteLine("5 Cooperative timer EasyMode example");
//            CooperativeTimer ct = new CooperativeTimer("Ilya");
//            ct.TimerTick += async (currTime) =>
//            {
//                Console.SetCursorPosition(0, 3);
//                Console.Write($"\r{currTime}");
//            };
//            ct.Notification += async (str) =>
//            {
//                await Task.Run(() =>
//                {
//                    Task.Delay(17);
//                    Console.SetCursorPosition(0, k++);
//                    Console.Write(str);
//                });
//            };

//            ct.SetTheme("Example 5 CooperativeTimer");

//            ct.SetEasyMode();

//            ct.StartTime("Vlad", "Please agree");
//            //await Task.Delay(12000);
//            //ct.StopTime();
//            Console.ReadLine();
//            Record record5 = ct.GetRecord();
//            Console.WriteLine($"Record: " +
//                 $"type {record5.Type} " +
//                 $"Theme {record5.Theme}, " +
//                 $"Time {record5.Time}, " +
//                 $"start date {record5.StartDate}, " +
//                 $"was it failed {record5.IsFailed}, " +
//                 $"failed Time {record5.FailedTime}");
//            break;

//        case ("6"):

//            Console.WriteLine("6 Timer HardMode plus cats use example");
//            Timer timer6 = new Timer();
//            timer6.TimerTick += async (currTime) =>
//            {
//                Console.SetCursorPosition(0, 3);
//                Console.Write($"\r{currTime}");
//            };
//            timer6.Notification += async (str) =>
//            {
//                await Task.Run(() =>
//                {
//                    Task.Delay(17);
//                    Console.SetCursorPosition(0, k++);
//                    Console.Write(str);
//                });
//            };

//            List<Process> processlist = ActivityObserver.GetInstance().GetAllOpenedWindows().ToList();

//            //foreach (Process process in processlist)
//            //{
//            //    if (!String.IsNullOrEmpty(process.MainWindowTitle))
//            //    {
//            //        Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
//            //    }
//            //}

//            Process catsMemes = processlist.Where(x => x.MainWindowTitle == "Мемы про котов - Поиск в Google - Google Chrome").ToList()[0];

//            ActivityObserver.GetInstance().AddAllowedApp(catsMemes);

//            timer6.SetTheme("Example 6");
//            timer6.SetTime(new TimeSpan(0, 1, 30));

//            timer6.SetHardMode();

//            timer6.StartTime();
//            await Task.Delay(new TimeSpan(0, 1, 32));
//            //timer6.StopTime();
//            Record record6 = timer6.GetRecord();
//            Console.WriteLine($"Record: " +
//                 $"type {record6.Type} " +
//                 $"Theme {record6.Theme}, " +
//                 $"Time {record6.Time}, " +
//                 $"start date {record6.StartDate}, " +
//                 $"was it failed {record6.IsFailed}, " +
//                 $"failed Time {record6.FailedTime}");
//            break;
//    }

//    Console.WriteLine("Repeat?(yes/no)");
//    string answer = Console.ReadLine();
//    if (answer == "yes")
//    {
//        repeat = true;
//        Console.Clear();
//    }
//    else if (answer == "no")
//    {
//        repeat = false;
//    }

//}


///*
////Client clientAPI = new Client();

////clientAPI.ConnectClient();
////clientAPI.StartListininServerAsync();
////clientAPI.SendMessage("Ilya\n");
////clientAPI.SendMessage("Test message1!\n");
////clientAPI.SendMessage("Test message2!\n");

////clientAPI.SendMessage("END!\n");



////LocaleStorageController localeStorageController = new LocaleStorageController();
////Console.WriteLine("Input your name");
////string name = Console.ReadLine();

////while (true)
////{

////    CooperativeTimerService cooperativeTimerService = new CooperativeTimerService(name);
////    CooperativeTimerRequest request = new CooperativeTimerRequest();
////    Console.WriteLine("Input to who you want to write");
////    request.UserName = Console.ReadLine();
////    Console.WriteLine("Input description");
////    request.Description = Console.ReadLine();
////    request.Theme = "Work";
////    request.Time = new TimeSpan(1, 0, 0);

////    cooperativeTimerService.CallUserCooperativeTimer(request);
////    Console.ReadLine();
////}
////CooperativeTimer mt = new CooperativeTimer("Ilya");



////int i = 0, j = 0;

////mt.TimerTick += async (currTime) =>
////{
////    Console.SetCursorPosition(0, 2);
////    Console.Write($"\r{currTime}");
////};
////int k = 6;
////mt.Notification += async (str) =>
////{
////    await Task.Run(() =>
////    {
////        Task.Delay(17);
////        Console.SetCursorPosition(0, k++);
////        Console.Write(str);
////    }
////    );

////};


////mt.SetTime(new TimeSpan(0, 1, 00));

////Console.ReadLine();
////mt.SetHardMode();
////mt.SetTheme("enjoing coffee!");

////mt.StartTime("Ilya2", "Test");

////Console.WriteLine();
////Console.WriteLine("1) Stop\n\n");
////string something = Console.ReadLine();

////Console.WriteLine();
////Console.WriteLine($"something {something}");
////if (something == "1")
////{
////    mt.StopTime();
////}

////Record rec = mt.GetRecord();

////Console.WriteLine($"Record {JsonSerializer.Serialize(rec)}");
////Console.ReadLine();
////Console.ReadLine();*/