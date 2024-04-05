using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestLike.Entities;


[Table("Users")]
public class User
{

    [PrimaryKey, AutoIncrement, Indexed]
    public int Id { get; set; }

    [Unique]
    public string Login { get; set; }
    public string HashPassword { get; set; }

}