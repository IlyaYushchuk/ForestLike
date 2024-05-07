using Serializer.Entities;

namespace ForestLike.Services;

public interface IStorageController
{
    public IEnumerable<Record> GetRecordsOfUser(int userId);
    public void UploadNewRecords(int userId, IEnumerable<Record> newRecords);
    public IEnumerable<string> GetUsersWithIds(IEnumerable<int> ids);
    public int UploadNewUser(User user);
    public User GetUserInfo(User user);
    public User GetUserInfo(int userId);
    public User GetUserInfo(string login);

}