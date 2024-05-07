

namespace Serializer.Entities;
public enum TimeCounterType
{
    Timer,
    StopWatch,
    CooperativeTimer
}
public class Record
{
    public TimeSpan Time { get; set; }
    public TimeCounterType Type { get; set; }
    public int RecordId { get; set; }
    public int UserId { get; set; }
    //public User? User { get; set; }
    public string Theme { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsFailed { get; set; }
    public TimeSpan FailedTime { get; set; }

}
