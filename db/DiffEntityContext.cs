using Microsoft.EntityFrameworkCore;

public class DiffEntityContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     {        
        optionsBuilder
        .UseNpgsql(AppConfiguration.Connection, o => o.SetPostgresVersion(9, 6))
        .UseLowerCaseNamingConvention();
    }

    public DbSet<DiffEntity> DiffEntities { get; set; }

}