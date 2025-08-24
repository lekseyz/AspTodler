using Infrastructure.Configurations;
using Infrastructure.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class TodlerDbContext : DbContext
{
    public TodlerDbContext(DbContextOptions<TodlerDbContext> options) : base(options) {}

    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}