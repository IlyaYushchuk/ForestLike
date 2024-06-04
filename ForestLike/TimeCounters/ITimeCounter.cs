using Serializer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestLike.TimeCounters;

public interface ITimeCounter
{
    public void StartTime();
    public string Theme {  get; set; }
    public TimeSpan Time { get; set; }
    public void StopTime();

    public event Action<string> Notification;
    public event Action<TimeSpan> TimerTick;
    public Record GetRecord();

}
