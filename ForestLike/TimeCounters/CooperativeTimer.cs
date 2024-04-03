using ForestLike.ClientServerLogic;
using ForestLike.Entities;
using Timer = System.Timers.Timer;

namespace ForestLike.TimeCounters;

public class CooperativeTimer:MainTimer
{
    Client client;

    public CooperativeTimer(Client client) : base()
    {
        this.client = client;
    }
}