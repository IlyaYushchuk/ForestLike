// See https://aka.ms/new-console-template for more information


using Server;
using Serializer.Entities;
using ServerNS;
using Server.DBServices;
using Serializer;
using System.Net.Sockets;





List<Record> records = new List<Record>
{
    new Record
    {
        Theme = "Theme1",
        Time = TimeSpan.FromHours(1),
        Type = TimeCounterType.Timer,
        RecordId = 1,
        UserId = 1,
        StartDate = DateTime.Now.AddDays(-1),
        IsFailed = false,
        FailedTime = TimeSpan.Zero
    },
    new Record
    {
        Theme = "Theme2",
        Time = TimeSpan.FromHours(2),
        Type = TimeCounterType.StopWatch,
        RecordId = 2,
        UserId = 1,
        StartDate = DateTime.Now.AddDays(-2),
        IsFailed = true,
        FailedTime = TimeSpan.FromMinutes(30)
    },
    // Добавьте остальные объекты аналогичным образом
    new Record
    {
        Theme = "Theme3",
        Time = TimeSpan.FromHours(3),
        Type = TimeCounterType.CooperativeTimer,
        RecordId = 3,
        UserId = 1,
        StartDate = DateTime.Now.AddDays(-3),
        IsFailed = false,
        FailedTime = TimeSpan.Zero
    },
    new Record
    {
        Theme = "Theme4",
        Time = TimeSpan.FromHours(4),
        Type = TimeCounterType.Timer,
        RecordId = 4,
        UserId = 1,
        StartDate = DateTime.Now.AddDays(-4),
        IsFailed = true,
        FailedTime = TimeSpan.FromMinutes(40)
    },
    new Record
    {
        Theme = "Theme5",
        Time = TimeSpan.FromHours(5),
        Type = TimeCounterType.StopWatch,
        RecordId = 5,
        UserId = 1,
        StartDate = DateTime.Now.AddDays(-5),
        IsFailed = false,
        FailedTime = TimeSpan.Zero
    },
    new Record
    {
        Theme = "Theme6",
        Time = TimeSpan.FromHours(6),
        Type = TimeCounterType.Timer,
        RecordId = 6,
        UserId = 1,
        StartDate = DateTime.Now.AddDays(-6),
        IsFailed = true,
        FailedTime = TimeSpan.FromMinutes(50)
    },
    new Record
    {
        Theme = "Theme7",
        Time = TimeSpan.FromHours(7),
        Type = TimeCounterType.StopWatch,
        RecordId = 7,
        UserId = 1,
        StartDate = DateTime.Now.AddDays(-7),
        IsFailed = false,
        FailedTime = TimeSpan.Zero
    },
    new Record
    {
        Theme = "Theme8",
        Time = TimeSpan.FromHours(8),
        Type = TimeCounterType.Timer,
        RecordId = 8,
        UserId = 1,
        StartDate = DateTime.Now.AddDays(-8),
        IsFailed = true,
        FailedTime = TimeSpan.FromMinutes(60)
    },
    new Record
    {
        Theme = "Theme9",
        Time = TimeSpan.FromHours(9),
        Type = TimeCounterType.CooperativeTimer,
        RecordId = 9,
        UserId = 1,
        StartDate = DateTime.Now.AddDays(-9),
        IsFailed = false,
        FailedTime = TimeSpan.Zero
    },
    new Record
    {
        Theme = "Theme10",
        Time = TimeSpan.FromHours(10),
        Type = TimeCounterType.Timer,
        RecordId = 10,
        UserId = 1,
        StartDate = DateTime.Now.AddDays(-10),
        IsFailed = true,
        FailedTime = TimeSpan.FromMinutes(70)
    }
};

List<User> us = new List<User>{
    new User {Id = 1, Login = "user1", HashPassword = "12345" },
    new User {Id = 2, Login = "user2", HashPassword = "23456" },
    new User {Id = 3, Login = "user3", HashPassword = "34567" },
    new User {Id = 4, Login = "user4", HashPassword = "45678" },
    new User {Id = 5, Login = "user5", HashPassword = "56789" }
    };


//Console.WriteLine(EntitySerializer.SerializeRecords(records));



ServerCommandHandler serverCommandHandler = new ServerCommandHandler();




Console.ReadLine();
Console.ReadLine();
Console.ReadLine();