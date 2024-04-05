
using SQLite;

namespace ForestLike.Entities;

public enum TimeCounterType
{
    Timer,
    StopWatch,
    CooperativeTimer
}

[Table("Records")]
public class Record
{
    public TimeSpan Time { get; set; }
    public TimeCounterType Type { get; set; }
    [PrimaryKey, AutoIncrement, Indexed]
    [Column("Id")]
    public int RecordId { get; set; }
    [Indexed]
    public int UserId { get; set; }
    public string Theme { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsFailed {  get; set; }
    public TimeSpan FailedTime { get; set; }
}
