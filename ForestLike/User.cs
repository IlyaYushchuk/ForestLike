using ForestLike.Entities;
using ForestLike.Services;
using ForestLike.TimeCounters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestLike;

public class User
{
    public string login {  get; set; }
    public string hashPassword { get; set; }
    public int Id { get; set; }

    IEnumerable<Record> records;
    IEnumerable<Record> newRecords;


    MainTimer mainTimer;

    IStorageController storageController;
    public User(IStorageController sc)
    {
        storageController = sc;
        mainTimer = new MainTimer();
        //mainTimer.NewRecordCreated += NewRecordAdditing;

    }

    private void NewRecordAdditing(Record newRecord)
    {
        newRecords.Append(newRecord);
    }

    public void Registration(string login, string hashPassword)
    {
        storageController.RegNewUser(login, hashPassword);
    }
    public void LogIn(string login, string hashPassword)
    {
        User user = storageController.GetUserInfo(login, hashPassword);
        Id = user.Id;
        records = user.records;
    }

    public void LogOut()
    {
        storageController.UploadNewRecords(Id, newRecords);
        //TODO other logout logic
    }
}
