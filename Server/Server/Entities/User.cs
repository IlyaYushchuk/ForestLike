using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string HashPassword { get; set; }
    public List<Record> Records { get; set; }

}
