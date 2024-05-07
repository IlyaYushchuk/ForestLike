using ForestLike.Services;
using Serializer.Entities;
using Serializer;

namespace Server.DBServices;

public class FileDBImmitation : IStorageController
{
    
    List<Record> records = new List<Record>();
    List<User> users = new List<User>();
    string usersFileName;
    string recordsFileName;
    string reserveUsersFileName;
    string reserveRecordsFileName;
    public FileDBImmitation()
    {
        reserveUsersFileName = "reserveUsers.txt";
        if (!File.Exists(reserveUsersFileName))
        {
            using (File.Create(reserveUsersFileName)) { }
        }
        usersFileName = "users.txt";
        if (!File.Exists(usersFileName))
        {
            using (File.Create(usersFileName)) { }
        }

        using (StreamReader sr = new StreamReader(usersFileName))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                users.Add(EntitySerializer.DeserializeUser(line));
            }
        }
        reserveRecordsFileName = "reserveRecords.txt";
        if (!File.Exists(reserveRecordsFileName))
        {
            using (File.Create(reserveRecordsFileName)) { }
        }
        recordsFileName = "records.txt";
        if (!File.Exists(recordsFileName))
        {
            using (File.Create(recordsFileName)) { }
        }

        using (StreamReader sr = new StreamReader(recordsFileName))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                records.Add(EntitySerializer.DeserializeRecord(line));
            }
        }

    }
    public IEnumerable<Record> GetRecordsOfUser(int userId)
    {
        return records.Where(r => r.UserId == userId); 
    }

    public User GetUserInfo(User user)
    {
        var selectedUser = users.FirstOrDefault(u => u.Login == user.Login && u.HashPassword == user.HashPassword);
        return selectedUser;
    }
    public User GetUserInfo(int userId)
    {
        var selectedUser = users.FirstOrDefault(u => u.Id == userId);
        return selectedUser;
    }

    public User GetUserInfo(string login)
    {
        var selectedUser = users.FirstOrDefault(u => u.Login == login);
        return selectedUser;
    }
    public IEnumerable<string> GetUsersWithIds(IEnumerable<int> ids)
    {
        var selectedUsers = users.Where(u => ids.Contains(u.Id));
        var selectedLogins = selectedUsers.Select(obj => obj.Login).ToList();
        return selectedLogins;
    }
    public void UploadNewRecords(int userId, IEnumerable<Record> newRecords)
    {
        int maxId = records.Max(r => r.RecordId);
        List<Record> rec = newRecords.ToList();
        using (StreamWriter sw = File.AppendText(recordsFileName))
        {
            foreach (Record r in rec)
            {
                r.UserId = userId;
                r.RecordId = ++maxId;
                sw.WriteLine(EntitySerializer.SerializeRecord(r));
                records.Add(r);
            }
        }
        using (StreamWriter sw = File.AppendText(reserveRecordsFileName))
        {
            foreach (Record r in rec)
            {
                r.UserId = userId;
                r.RecordId = ++maxId;
                sw.WriteLine(EntitySerializer.SerializeToXmlString(r));
                records.Add(r);
            }
        }
    }

    public int UploadNewUser(User user)
    {
        int maxId = users.Max(u => u.Id);
        user.Id = maxId+1;
        using (StreamWriter sw = File.AppendText(usersFileName))
        {
            sw.WriteLine(EntitySerializer.SerializeUser(user));
            users.Add(user);    
        }
        using (StreamWriter sw = File.AppendText(reserveUsersFileName))
        {

            sw.WriteLine(EntitySerializer.SerializeToXmlString(user));
            users.Add(user);
        }
        return user.Id;
    }
}
