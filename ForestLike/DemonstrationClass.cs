using ForestLike.ClientServerLogic;
using ForestLike.Services;
using Serializer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestLike;

public class DemonstrationClass
{
    Client client;
    ServerRequestSender serverRequestSender;
    AuthorizationService authorizationService;

    public DemonstrationClass()
    {
        client = new Client();
        serverRequestSender = new ServerRequestSender(client);
        authorizationService = new AuthorizationService(client);
    }

    public void Demonstrate()
    {
        Console.WriteLine("Success!!!");

        bool repeat = true;
        while (repeat)
        {
            Console.WriteLine("Input number of use-case");
            string selectedCase = Console.ReadLine();
            int k = 6;
            List<User> users = new List<User> {
                new User {Login = "Genry", HashPassword = "password" },
                new User {Login = "Steven", HashPassword = "3425" },
                new User {Login = "user1", HashPassword = "12345" },
                new User {Login = "Ilya", HashPassword = "ASD-31da-asd@sS3" },
                new User {Login = "Kostya", HashPassword = "mkpassword" },
                new User {Login = "NotExistingUser", HashPassword = "12345" }
            };

            Record record = new Record { Theme = "OOoooooy My GOOOOOOD!!!", Time = new TimeSpan(1, 0, 0), Type = TimeCounterType.StopWatch, RecordId = 1, UserId = 10, StartDate = DateTime.Now, IsFailed = false, FailedTime = new TimeSpan(0, 0, 0) };


            List<Record> rec = new List<Record>();
            rec.Add(record);


            switch (selectedCase)
            {
                case ("1"):
                    Console.WriteLine("1 Registration example 1 ");

                    authorizationService.Register(users[1]);
                    break;
                case ("2"):
                    Console.WriteLine("2 Registration example 2 ");
                    authorizationService.Register(users[0]);
                    break;
                case ("3"):
                    Console.WriteLine("3 Login example 1 ");
                    authorizationService.Login(users[0]);
                    break;
                case ("4"):
                    Console.WriteLine("4 Login example 2 ");
                    authorizationService.Login(users[5]);
                    break;
                case ("5"):
                    Console.WriteLine("5 Login as enother user");
                    authorizationService.Login(users[2]);

                    break;
                case ("6"):
                    Console.WriteLine("6 Upload new record example");
                    serverRequestSender.UploadNewRecords(rec);
                    break;

                case ("7"):
                    Console.WriteLine("7 Get all online users example");

                    List<string> names = serverRequestSender.GetAllOnlineUsers().Result.ToList();
                    foreach (string name in names)
                    {
                        Console.WriteLine(name);
                    }
                    break;
            }

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
    }
}
