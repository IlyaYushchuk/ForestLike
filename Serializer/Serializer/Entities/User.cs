

namespace Serializer.Entities;

public class User
{
    public int Id { get; set; } = 0;
    public string Login { get; set; } = "";
    public string HashPassword { get; set; } = "";
    public List<Record> Records { get; set; } = null;
}
