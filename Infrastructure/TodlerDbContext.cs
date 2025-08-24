using Infrastructure.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class TodlerDbContext : DbContext
{
    DbSet<RefreshToken> RefreshTokens { get; set; }
    DbSet<User> Users { get; set; }
}