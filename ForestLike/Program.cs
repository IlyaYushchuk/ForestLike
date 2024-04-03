using ForestLike;
using ForestLike.Entities;
using ForestLike.TimeCounters;
using ForestLike.ClientServerLogic;

//Client client = new Client();

//client.ConnectClient();
//client.StartListininServerAsync();
//client.SendMessage("Ilya\n");
//client.SendMessage("Test message1!\n");
//client.SendMessage("Test message2!\n");

//client.SendMessage("END!\n");
StopWatch mt = new StopWatch();



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




Console.ReadLine();
mt.SetEasyMode();
mt.SetTheme("enjoing coffee!");
mt.StartTime();
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

//Console.WriteLine($"Record {rec.FailedTime}");
Console.ReadLine();
Console.ReadLine();