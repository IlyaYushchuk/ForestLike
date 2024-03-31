
using ForestLike.Entities;

namespace ForestLike;

public class Statistic
{
    IEnumerable<Record> records;

    public void AddNewRecord(Record newRecord)
    {
        records.Append(newRecord);
    }
    public Statistic(IEnumerable<Record> records)
    {
        this.records = records;
    }

    public IEnumerable<Record> StatisticForDay(DateOnly date)
    {
        return records.Where(r => DateOnly.FromDateTime(r.StartDate).Equals(date));
    }

}
