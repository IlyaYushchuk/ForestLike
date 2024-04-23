using Microsoft.EntityFrameworkCore;
using Server.Entities;

namespace Server;

public class ApplicationDBContext:DbContext
{
    public DbSet<User> Users { get; private set; }
    public DbSet<Record> Records { get; private set; }

    public ApplicationDBContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ForestLikeTempDB.db");
    }
}
