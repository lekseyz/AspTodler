using Domain.Models;
using Persistence.Configurations;
using Persistence.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class TodlerDbContext : DbContext
{
    public TodlerDbContext(DbContextOptions<TodlerDbContext> options) : base(options) {}

    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<NoteInfoEntity> NoteInfos { get; set; }
    public DbSet<NoteContentEntity> NoteContents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}