using ForestLike.Entities;
using Timer = System.Timers.Timer;

namespace ForestLike.TimeCounters;

public class StopWatch: ITimeCounter
{
    protected TimeSpan _time;
    protected string _theme = "";

    protected Timer everySecTimer = new Timer(new TimeSpan(0, 0, 1));
    protected TimeSpan currTime;

    protected Record currRecord = new Record();

    //TODO rename event
    public event Action<string> Notification;
    public event Action<TimeSpan> TimerTick;
    public StopWatch()
     {
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
    public void StartTime()
    {
        currRecord.StartDate = DateTime.Now;

        currTime = new TimeSpan(0, 0, 0);

        everySecTimer.Start();
        ActivityObserver.GetInstance().StartObserveTimer();

    }
    public void StopTime()
    {
        currRecord.IsFailed = false;
        currRecord.FailedTime = new TimeSpan(0,0,0);
        currRecord.Time = currTime;

        everySecTimer.Stop();
        ActivityObserver.GetInstance().StopObserveTimer();
        Notification?.Invoke("You stoped stopwatch. Congratulations");
    }

    public void SetEasyMode()
    {
        ActivityObserver.GetInstance().NotAllowedAppUsed -= HardMode;
        ActivityObserver.GetInstance().NotAllowedAppUsed += EasyMode;
    }

    public Record GetRecord()
    {
        //TODO check does curr record has been intialized properly
        return currRecord;
    }

    protected void EasyMode()
    {
        Notification?.Invoke("You lose concentration. Please come back to ForestLike app.");
    }
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
        Notification?.Invoke("Timer have been failed. Try again!!!");
        ActivityObserver.GetInstance().StopObserveTimer();
    }
}
