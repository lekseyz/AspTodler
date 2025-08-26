using Domain.Models;
using Domain.ValueTypes;
using Persistence.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UserRepository
{
    private readonly TodlerDbContext _context;

    public UserRepository(TodlerDbContext context)
    {
        _context = context;
    }

    public async Task Add(User user)
    {
        var password = user.Password;
        var userEntity = new UserEntity()
        {
            Id = user.Id,
            Email = user.Email,
            PasswordHash = password.Hash,
            PasswordSalt = password.Salt,
        };
        
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<User>> GetAll()
    {
        var userEntities = await _context.Users.AsNoTracking().ToListAsync();
        
        return userEntities.Select(u => User.Construct(u.Id, u.Email, new Password(u.PasswordHash, u.PasswordSalt))).ToList();
    }

    public async Task<User?> GetById(Guid id)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (userEntity is null)
            return null;

        var userResult = User.Construct(userEntity.Id, userEntity.Email,
            new Password(userEntity.PasswordHash, userEntity.PasswordSalt));
        return userResult;
    }

    public async Task<User?> GetByEmail(string email)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (userEntity is null)
            return null;
        
        var userResult = User.Construct(userEntity.Id, userEntity.Email,
            new Password(userEntity.PasswordHash, userEntity.PasswordSalt));
        return userResult;
    }

    public async Task<bool> IsEmailPresent(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email);
    }

    public async Task Delete(Guid id)
    {
        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        _context.Users.Remove(user);
        _context.RefreshTokens.RemoveRange(user.RefreshTokens);
        await _context.SaveChangesAsync();
    }

    private async Task<bool> UserExists(Guid userId)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId);
    }
}