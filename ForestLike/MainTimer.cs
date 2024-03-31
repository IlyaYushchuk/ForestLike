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

    //CancellationTokenSource cancellationTokenSource;
    //CancellationToken cancellationToken;

    Record currRecord = new Record();

    int easytModCounter = 0;
    int maxMistakeCount = 10;

    //TODO rename event
    public event Action<string> Notification;
    public event Action<TimeSpan> TimerTick;

    public MainTimer()//CancellationTokenSource cancellationTokenSource) 
    {
        //this.cancellationTokenSource = cancellationTokenSource;
        //cancellationToken = cancellationTokenSource.Token;

        timer.AutoReset = false;
        timer.Elapsed += (source, e) =>
        {
            currTime = currTime.Add(new TimeSpan(0, 0, 1));
            TimerTick?.Invoke(currTime);
            everySecTimer.Stop();
            Notification?.Invoke("Timer ended successfully. Congratulations!!!");

            currRecord.IsFailed = false;
            currRecord.FailedTime = new TimeSpan(0, 0, 0);
           
            ActivityObserver.GetInstance().StopObserveTimer(); 
        };

        everySecTimer.AutoReset = true;
        everySecTimer.Elapsed += (s, e) =>
        {
            TimerTick?.Invoke(currTime);
            currTime = currTime.Add(new TimeSpan(0, 0, 1));
        };
        SetEasyMode();
    }

    public void SetTheme(string theme)
    {
        _theme = theme;
        currRecord.Theme = _theme;
    }
    public void SetTime(TimeSpan time)
    {
        _time = time;
        timer = new Timer(_time);
        currRecord.Time = _time;
    }

    //TODO rename hard mode
    public void SetHardMode()
    {
        ActivityObserver.GetInstance().NotAllowedAppUsed += () =>
        {
            currRecord.IsFailed = true;
            currRecord.FailedTime = currTime;
            everySecTimer.Stop();
            timer.Stop();
            Notification?.Invoke("Timer have been failed. Try again!!!");
        };
        ActivityObserver.GetInstance().StopObserveTimer();
    }

    public void SetEasyMode()
    {
        ActivityObserver.GetInstance().NotAllowedAppUsed += () =>
        {
            if (easytModCounter == maxMistakeCount)
            {
                currRecord.IsFailed = true;
                currRecord.FailedTime = currTime;
                everySecTimer.Stop();
                timer.Stop();
                Notification?.Invoke("Timer have been failed. Try again!!!");
            }
            else
            {
                Notification?.Invoke("You lose concentration. Please come back to ForestLike app.");
                easytModCounter++;
            }
        };
        ActivityObserver.GetInstance().StopObserveTimer();
    }

    public void StartTime()
    {        
        currRecord.StartDate = DateTime.Now;

        currTime = new TimeSpan(0, 0, 0);
        
        everySecTimer.Start();
        timer.Start();
        ActivityObserver.GetInstance().StartObserveTimer();
    }

    public Record GetPrevRecord()
    {
        //TODO check does curr record has been intialized properly
        return currRecord;
    }

    public void StopTime()
    {
        currRecord.IsFailed = true;
        currRecord.FailedTime = currTime;

        everySecTimer.Stop();
        timer.Stop();
        ActivityObserver.GetInstance().StopObserveTimer();
        Notification?.Invoke("You stoped timer.");
    }
}
