using ForestLike.ClientServerLogic;
using ForestLike.Entities;
using ForestLike.Services;
using System.Runtime.CompilerServices;
using Timer = System.Timers.Timer;

namespace ForestLike.TimeCounters;

public class CooperativeTimer:Timer
{
    CooperativeTimerService cooperativeTimerService;

    bool callSended = false;
    //TODO event cant be inherited. I think so
    public new event Action<string> Notification;
    public new event Action<TimeSpan> TimerTick;
    public CooperativeTimer(string name):base()
    {

        currRecord.Type = TimeCounterType.CooperativeTimer;
        cooperativeTimerService = new CooperativeTimerService(name);
        cooperativeTimerService.ServerCommandEvent += CommandHandler;


        timer.AutoReset = false;
        timer.Elapsed += (source, e) =>
        {
            TimerTick?.Invoke(currTime);
            Notification?.Invoke("Timer ended successfully. Congratulations!!!");
        };

        everySecTimer.AutoReset = true;
        everySecTimer.Elapsed += (s, e) =>
        {
            TimerTick?.Invoke(currTime);
        };
    }
   

    private void CommandHandler(string message)
    {  
        if (message == "/YES")
        {
            if (callSended)
            {
                Notification?.Invoke("User Agreed on cooperative timer");
                base.startTime();
                Notification?.Invoke("Timer started");
            }
        }
        else if(message == "/NO")
        {
            if(callSended)
            {
                callSended = false;
            }

        }else if(message == "/FAIL")
        {
            stopTime();
            callSended = false;
        }
    }


    public new void StopTime()
    {
        base.stopTime();
        Notification?.Invoke("You stoped timer.");
    }
    protected new void StartTime()
    {

    }
    public void StartTime(string userName, string description)
    {
        SendCallToUser(userName, description);
        Notification?.Invoke($"Call to {userName} sended");
    }

    private void SendCallToUser(string userName,string description)
    {
        callSended = true;
        CooperativeTimerRequest cooperativeTimerRequest = new CooperativeTimerRequest
        {
            UserName = userName,
            Time = time,
            Theme = theme,
            Description = description
        };
        cooperativeTimerService.CallUserCooperativeTimer(cooperativeTimerRequest);
    }

    protected override void EasyMode()
    {
        Notification?.Invoke("You lose concentration. Please come back to ForestLike app.");
    }

  
    protected override void HardMode()
    {
        currRecord.IsFailed = true;
        currRecord.FailedTime = currTime;
        everySecTimer.Stop();
        timer.Stop();
        Notification?.Invoke("Cooperative timer.Timer have been failed. Try again!!!");
        ActivityObserver.GetInstance().StopObserveTimer();

    }
}