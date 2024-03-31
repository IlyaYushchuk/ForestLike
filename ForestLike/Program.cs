
using ForestLike;
using System.Diagnostics;
using System.Timers;

Console.WriteLine("Hello, World!");

//ActivityObserver observer = new ActivityObserver();

//observer.GetAllOpenedWindows();
MainTimer mt  = new MainTimer();
//mt.observer.GetAllOpenedWindows();
int k = 40;

mt.SetTime(new TimeSpan(0, 0, k));

TimeSpan min10 = new TimeSpan(0, 0, k);
mt.TimerTick += (currTime) =>
{
    Console.SetCursorPosition(0, 2);
    Console.Write($"\r{min10 - currTime}");
};

DateTime startTime = DateTime.Now;
System.Timers.Timer timer = new System.Timers.Timer(new TimeSpan(0,0,1));
timer.Elapsed += (source, e) =>
{
    Console.SetCursorPosition(0, 1);
    Console.Write($"\r{e.SignalTime.Subtract(startTime)}");
};
timer.AutoReset = true;

//timer.Start();
Console.ReadLine();
//mt.observer.KostilFunc();
Console.ReadLine();



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
Console.ReadLine();
Console.ReadLine();