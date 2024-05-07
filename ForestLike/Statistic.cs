
using ForestLike.ClientServerLogic;
using Serializer;
using Serializer.Entities;
using System.Collections.Generic;

namespace ForestLike;

public class Statistic
{
    IEnumerable<Record> records;
    private static Statistic instance;

    public object EntitySerizlizer { get; private set; }

    public void AddNewRecord(Record newRecord)
    {
        records.Append(newRecord);
    }
    public void AddNewRecords(IEnumerable<Record> newRecords)
    {
        var listRecord = records.ToList();
        foreach (var r in newRecords)
        {
            listRecord.Add(r);
        }
        records = listRecord;
    }
    public static Statistic GetInstance()
    {
        if (instance == null)
        {
            instance = new Statistic();
        }
        return instance;
    }
    private Statistic()
    {
        records = new List<Record>();
    }

    public void Show ()
    {
        var Records = records.ToList();
        foreach (Record r in Records) 
        {
            Console.WriteLine(EntitySerializer.SerializeRecord(r));
        }
    }
    public IEnumerable<Record> StatisticForDay(DateOnly date)
    {
        return records.Where(r => DateOnly.FromDateTime(r.StartDate).Equals(date));
    }

}
