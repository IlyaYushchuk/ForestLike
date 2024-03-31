using ForestLike.Entities;

namespace ForestLike.Services;

public interface IStorageController
{
    public IEnumerable<Record> GetRecordsOfUser(int userId);
    public void UploadNewRecords(int userId, IEnumerable<Record> newRecords);

    public void RegNewUser(string login, string password);
    public User GetUserInfo(string login, string password);
    public string SerializeRecord(Record record);
    public Record DeserializeRecord(string record);
    public string SerializeUser(User user);
    public User DeserializeUser(string user);
    public string GetUserIpByName(string name);
}