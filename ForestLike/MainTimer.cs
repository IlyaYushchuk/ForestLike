using ForestLike.Entities;
using Timer = System.Timers.Timer;

namespace ForestLike;


//TODO Class is to big may be I need to split it

public class MainTimer : ITimeCounter
{
    TimeSpan _time;
    string _theme = "";
    Timer timer;
    Timer everySecTimer = new Timer(new TimeSpan(0, 0, 1));
    TimeSpan currTime;
    CancellationTokenSource cancellationTokenSource;
    CancellationToken cancellationToken;

    TimeSpan ActivCheckPeriod = new TimeSpan(0, 0, 1);


    Record currRecord = new Record();


    //TODO rename event
    public event Action FailTimer;
    public event Action<TimeSpan> TimerTick;
    public event Action<Record> NewRecordCreated;
    public MainTimer(CancellationTokenSource cancellationTokenSource) 
    {
        this.cancellationTokenSource = cancellationTokenSource;
        cancellationToken = cancellationTokenSource.Token;

        everySecTimer.AutoReset = true;
        everySecTimer.Elapsed += (s, e) =>
        {
            TimerTick?.Invoke(currTime);
            currTime = currTime.Add(new TimeSpan(0, 0, 1));
        };
    }
    public void SetTheme(string theme)
    {
        _theme = theme;
    }
    public void SetTime(TimeSpan time)
    {
        _time = time;
    }

    //TODO rename hard mode
    public void SetHardMode()
    {
        everySecTimer.Elapsed += (s, e) =>
        {
            if (cancellationToken.IsCancellationRequested)
            {
                currRecord.IsFailed = true;
                currRecord.FailedTime = currTime;
                everySecTimer.Stop();
                timer.Stop();
                Console.WriteLine("Timer have been failed. Try again!!!");
            }
        };
    }

    public void SetEasyMode()
    {
        
    }

    public void StartTime()
    {
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        currRecord.StartDate = DateTime.Now;

        currTime = new TimeSpan(0, 0, 0);
        
        timer = new Timer(_time);
        timer.AutoReset = false;


        
       
        timer.Elapsed += (source, e) => 
        {
            currTime = currTime.Add(new TimeSpan(0, 0, 1));
            TimerTick?.Invoke(currTime); 
            everySecTimer.Stop();
            Console.WriteLine("Timer ended successfully. Congratulations!!!");
            currRecord.Theme = _theme;
            currRecord.Time = _time;
            currRecord.IsFailed = false;
            currRecord.FailedTime = new TimeSpan(0,0,0);

            NewRecordCreated?.Invoke(currRecord);
        };
        everySecTimer.Start();
        timer.Start();
    }

    public void StopTime()
    {
        Console.WriteLine("Im here");
        cancellationTokenSource.Cancel();

        currRecord.Theme = _theme;
        currRecord.Time = _time;
        currRecord.IsFailed = true;
        currRecord.FailedTime = currTime;

        everySecTimer.Stop();
        timer.Stop();

        NewRecordCreated?.Invoke(currRecord);

        Console.WriteLine("You have been failed.");
    }
}
