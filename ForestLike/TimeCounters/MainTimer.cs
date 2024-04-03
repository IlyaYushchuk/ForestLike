using ForestLike.Entities;
using Timer = System.Timers.Timer;

namespace ForestLike.TimeCounters;


//TODO Class is to big may be I need to split it

public class MainTimer : ITimeCounter
{
    protected TimeSpan _time;
    protected string _theme = "";

    protected Timer timer = new Timer(new TimeSpan(0, 10, 0));
    protected Timer everySecTimer = new Timer(new TimeSpan(0, 0, 1));
    protected TimeSpan currTime;

    //CancellationTokenSource cancellationTokenSource;
    //CancellationToken cancellationToken;

    protected Record currRecord = new Record();

 

    //TODO rename event
    public event Action<string> Notification;
    public event Action<TimeSpan> TimerTick;


    public MainTimer()
    {
        timer.AutoReset = false;
        timer.Elapsed += (source, e) =>
        {
            currTime = currTime.Add(new TimeSpan(0, 0, 1));
            TimerTick?.Invoke(currTime);
            everySecTimer.Stop();
            Notification?.Invoke("Timer ended successfully. Congratulations!!!");

            currRecord.IsFailed = false;
            currRecord.FailedTime = new TimeSpan(0, 0, 0);
            currRecord.Time = _time;

            ActivityObserver.GetInstance().StopObserveTimer();
        };

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
        currRecord.Theme = _theme;
    }
    public void SetTime(TimeSpan time)
    {
        _time = time;
        timer.Interval = time.TotalMilliseconds;
        currRecord.Time = _time;
    }

    //TODO rename hard mode
    public void SetHardMode()
    {
        ActivityObserver.GetInstance().NotAllowedAppUsed -= EasyMode;
        ActivityObserver.GetInstance().NotAllowedAppUsed += HardMode;
    }
    protected void HardMode()
    {
        currRecord.IsFailed = true;
        currRecord.FailedTime = currTime;
        everySecTimer.Stop();
        timer.Stop();
        Notification?.Invoke("Timer have been failed. Try again!!!");
        ActivityObserver.GetInstance().StopObserveTimer();
    }

    protected void EasyMode()
    {
        Notification?.Invoke("You lose concentration. Please come back to ForestLike app.");
    }

    public void SetEasyMode()
    {
        ActivityObserver.GetInstance().NotAllowedAppUsed -= HardMode;
        ActivityObserver.GetInstance().NotAllowedAppUsed += EasyMode;
    }



    public void StartTime()
    {
        Console.WriteLine("StartTime");
        
        currRecord.StartDate = DateTime.Now;

        currTime = new TimeSpan(0, 0, 0);

        everySecTimer.Start();
        timer.Start();
        ActivityObserver.GetInstance().StartObserveTimer();
    }

    public Record GetRecord()
    {
        //TODO check does curr record has been intialized properly
        return currRecord;
    }

    public virtual void StopTime()
    {
        currRecord.IsFailed = true;
        currRecord.FailedTime = currTime;

        everySecTimer.Stop();
        timer.Stop();
        ActivityObserver.GetInstance().StopObserveTimer();
        Notification?.Invoke("You stoped timer.");
    }
}
