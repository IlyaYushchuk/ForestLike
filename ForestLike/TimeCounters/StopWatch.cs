using Serializer.Entities;
using EmbededTimer = System.Timers.Timer;

namespace ForestLike.TimeCounters;

public class StopWatch: ITimeCounter
{
    

    protected EmbededTimer everySecTimer = new EmbededTimer(new TimeSpan(0, 0, 1));
    protected TimeSpan currTime;

    protected Record currRecord = new Record();

    public string Theme { get; set; }
    public TimeSpan Time { get; set; }

    //TODO rename event
    public event Action<string> Notification;
    public event Action<TimeSpan> TimerTick;
    public StopWatch()
     {
        currRecord.Type = TimeCounterType.StopWatch;
        everySecTimer.AutoReset = true;
        everySecTimer.Elapsed += (s, e) =>
        {
            currTime = currTime.Add(new TimeSpan(0, 0, 1));
            TimerTick?.Invoke(currTime);
        };
     }

  
    public void StartTime()
    {
        startTime();
      //  Notification?.Invoke("Stopwatch started");
    }
    protected void startTime()
    {
        currRecord.StartDate = DateTime.Now;
        currRecord.Theme = Theme;
        currTime = new TimeSpan(0, 0, 0);

        everySecTimer.Start();
        ActivityObserver.GetInstance().StartObserveTimer();

    }

    public void StopTime()
    {
        stopTime();
        Notification?.Invoke("You stoped stopwatch. Congratulations");
    }
    protected void stopTime()
    {
        currRecord.IsFailed = false;
        currRecord.FailedTime = new TimeSpan(0,0,0);
        currRecord.Time = currTime;
        

        everySecTimer.Stop();
        ActivityObserver.GetInstance().StopObserveTimer();
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
