using Serializer.Entities;
using EmbededTimer = System.Timers.Timer;

namespace ForestLike.TimeCounters;


//TODO Class is to big may be I need to split it

public class Timer : ITimeCounter
{
    protected TimeSpan time;
    protected string theme = "";

    protected EmbededTimer timer = new EmbededTimer(new TimeSpan(0, 10, 0));
    protected EmbededTimer everySecTimer = new EmbededTimer(new TimeSpan(0, 0, 1));
    protected TimeSpan currTime;

    //CancellationTokenSource cancellationTokenSource;
    //CancellationToken cancellationToken;

    protected Record currRecord = new Record();

 

    //TODO rename event
    public event Action<string> Notification;
    public event Action<TimeSpan> TimerTick;


    public Timer()
    {
        timer.AutoReset = false;
        timer.Elapsed += (source, e) =>
        {
            everySecTimer.Stop();
            Notification?.Invoke("Timer ended successfully. Congratulations!!!");

            currRecord.IsFailed = false;
            currRecord.FailedTime = new TimeSpan(0, 0, 0);
            currRecord.Time = time;

            ActivityObserver.GetInstance().StopObserveTimer();
        };

        everySecTimer.AutoReset = true;
        everySecTimer.Elapsed += (s, e) =>
        {
            currTime = currTime.Add(new TimeSpan(0, 0, 1));
            TimerTick?.Invoke(currTime);
        };
    }

    public void SetTheme(string theme)
    {
        this.theme = theme;
        currRecord.Theme = this.theme;
    }
    public void SetTime(TimeSpan time)
    {
        this.time = time;
        timer.Interval = time.TotalMilliseconds;
        currRecord.Time = this.time;
    }

    //TODO rename hard mode
    public void SetHardMode()
    {
        ActivityObserver.GetInstance().NotAllowedAppUsed -= EasyMode;
        ActivityObserver.GetInstance().NotAllowedAppUsed += HardMode;
    }
    protected virtual void HardMode()
    {
        currRecord.IsFailed = true;
        currRecord.FailedTime = currTime;
        everySecTimer.Stop();
        timer.Stop();
        Notification?.Invoke("Timer have been failed. Try again!!!");
        ActivityObserver.GetInstance().StopObserveTimer();
    }

    protected virtual void EasyMode()
    {
        Notification?.Invoke("You lose concentration. Please come back to ForestLike app.");
    }

    public void SetEasyMode()
    {
        ActivityObserver.GetInstance().NotAllowedAppUsed -= HardMode;
        ActivityObserver.GetInstance().NotAllowedAppUsed += EasyMode;
    }


    public virtual void StartTime()
    {
        Notification?.Invoke("Timer started");
        startTime();
    }
    protected void startTime()
    {
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
        stopTime();
        Notification?.Invoke("You stoped timer.");
    }
    protected void stopTime()
    {
        currRecord.IsFailed = true;
        currRecord.FailedTime = currTime;

        everySecTimer.Stop();
        timer.Stop();
        ActivityObserver.GetInstance().StopObserveTimer();
     
    }
}
