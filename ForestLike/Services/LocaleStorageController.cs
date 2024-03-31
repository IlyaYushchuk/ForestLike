using ForestLike.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestLike.Services;

public class LocaleStorageController : IStorageController
{
    public Record DeserializeRecord(string record)
    {
        throw new NotImplementedException();
    }
    public User DeserializeUser(string user)
    {
        throw new NotImplementedException();
    }
    public void RegNewUser(string login, string password)
    {
        throw new NotImplementedException();

    }
    public IEnumerable<Record> GetRecordsOfUser(int userId)
    {

        throw new NotImplementedException();
    }

    public User GetUserInfo(string login, string password)
    {
        throw new NotImplementedException();
    }

    public string GetUserIpByName(string name)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}
