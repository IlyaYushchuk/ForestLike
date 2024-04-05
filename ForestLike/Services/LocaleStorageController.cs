using ForestLike.Entities;      
using SQLite ;

namespace ForestLike.Services;

public class LocaleStorageController : IStorageController
{
    SQLiteConnection connection;
    public LocaleStorageController()
    {
        connection = new SQLiteConnection(Path.Combine(Directory.GetCurrentDirectory(), "MyDB.db"));
     
    }
    public Record DeserializeRecord(string record)
    {
        throw new NotImplementedException();
    }
    public User DeserializeUser(string user)
    {
        throw new NotImplementedException();
    }
    public void UploadNewUser(User user)
    {
        connection.Insert(user);
    }
    public IEnumerable<Record> GetRecordsOfUser(int userId)
    {
        return connection.Table<Record>().Where(r => r.UserId == userId).ToList();
    }

    public User GetUserInfo(User user)
    {
        return connection.Table<User>().Where(u => u.Login == user.Login && u.HashPassword == user.HashPassword).ToList()[0];
    }


    public string SerializeRecord(Record record)
    {
        throw new NotImplementedException();
    }

    public string SerializeUser(User user)
    {
        throw new NotImplementedException();
    }

    public void UploadNewRecords(int userId, IEnumerable<Record> newRecords)
    {
        List<Record> records = newRecords.ToList();
        foreach (Record record in records) 
        {
            record.UserId = userId;
            connection.Insert(record);
        }
    }

}
