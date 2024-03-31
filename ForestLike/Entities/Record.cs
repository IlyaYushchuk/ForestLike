
namespace ForestLike.Entities;

public class Record
{
    public TimeSpan Time { get; set; }
    public int Id { get; set; }
    public string Theme { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsFailed {  get; set; }
    public TimeSpan FailedTime { get; set; }
}
