using Domain.Models;
using Infrastructure.DbEntities;
using Microsoft.EntityFrameworkCore;
using Misc;

namespace Infrastructure.Repositories;

public class UserRepository
{
    private readonly TodlerDbContext _context;

    public UserRepository(TodlerDbContext context)
    {
        _context = context;
    }

    public async Task Add(UserModel user)
    {
        var userEntity = new UserEntity()
        {
            Email = user.Email,
            Guid = user.Id,
            PasswordHash = user.PasswordHash,
            PasswordSalt = user.PasswordSalt,
        };
        
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<UserEntity>> GetAll()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<ResultOptional<UserModel>> GetById(Guid id)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Guid == id);
        
        if (userEntity == null) 
            return ResultOptional<UserModel>.Failure(Error.NotFound($"User with id {id} not found"));
        
        var userResult = new UserModel(userEntity.Guid, userEntity.Email, userEntity.PasswordHash,  userEntity.PasswordSalt);
        
        return ResultOptional<UserModel>.Success(userResult);
    }

    public async Task<ResultOptional<UserModel>> GetByEmail(string email)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
        
        if (user is null) 
            return ResultOptional<UserModel>.Failure(Error.NotFound($"User with email {email} not found"));
        
        var userResult = new UserModel(user.Guid, user.Email, user.PasswordHash, user.PasswordSalt);
        
        return ResultOptional<UserModel>.Success(userResult);
    }

    public async Task<bool> IsEmailPresent(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email);
    }

    public async Task<Result> Delete(Guid id)
    {
        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Guid == id);
        
        if (user is null) 
            return Result.Failure(Error.NotFound($"Cannot delete user. User {id} not found"));
        
        _context.Users.Remove(user);
        _context.RefreshTokens.RemoveRange(user.RefreshTokens);
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    private async Task<bool> UserExists(Guid userId)
    {
        return await _context.Users.AnyAsync(u => u.Guid == userId);
    }
}