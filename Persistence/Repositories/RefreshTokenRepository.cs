using System.Runtime.InteropServices.JavaScript;
using Domain.ValueTypes;
using Persistence.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories;

public class RefreshTokenRepository
{
    private TodlerDbContext _context;
    private ILogger<RefreshTokenRepository> _logger;
    public RefreshTokenRepository(TodlerDbContext context, ILogger<RefreshTokenRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Check(Guid userGuid, string token)
    {
        return await _context.RefreshTokens.AsNoTracking()
            .AnyAsync(t =>
                t.Token == token &&
                t.User.Id == userGuid &&
                t.IsActive &&
                t.Expires > DateTime.UtcNow
            );
    }

    public async Task Update(Guid userGuid, string oldToken, RefreshToken newToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userGuid);
        
        var token = await _context.RefreshTokens
            .Include(t => t.User)
            .Where(t => t.Token == oldToken && t.UserId == user!.Id)
            .FirstOrDefaultAsync(); 

        var nextToken = RefreshTokenEntity.Create(token!.User, newToken);
        await _context.RefreshTokens
            .AddAsync(nextToken);
        
        token.IsActive = false;
        token.NextToken = nextToken;
        
        await _context.SaveChangesAsync();
    }

    public async Task Add(Guid userGuid, RefreshToken newToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userGuid);
        
        await _context.RefreshTokens.AddAsync(RefreshTokenEntity.Create(user!, newToken));
        await _context.SaveChangesAsync();
    }
}