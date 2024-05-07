using Serializer.Entities;      
using SQLite ;

namespace ForestLike.Services;

public class LocaleStorageController : IStorageController
{
    SQLiteConnection connection;
    public LocaleStorageController()
    {
        connection = new SQLiteConnection(Path.Combine(Directory.GetCurrentDirectory(), "MyDB.db"));
     
    }
    public IEnumerable<string> GetUsersWithIds(IEnumerable<int> ids)
    {
        throw new NotImplementedException();
    }
    public int UploadNewUser(User user)
    {
        connection.Insert(user);
        return user.Id;
    }
    public IEnumerable<Record> GetRecordsOfUser(int userId)
    {
        return connection.Table<Record>().Where(r => r.UserId == userId).ToList();
    }

    public User GetUserInfo(User user)
    {
        return connection.Table<User>().Where(u => u.Login == user.Login && u.HashPassword == user.HashPassword).ToList()[0];
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

    public User GetUserInfo(int userId)
    {
        throw new NotImplementedException();
    }

    public User GetUserInfo(string login)
    {
        throw new NotImplementedException();
    }
}
