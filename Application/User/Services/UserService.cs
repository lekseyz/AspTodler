using Application.User.Contracts;
using Application.User.Interfaces;
using Domain.Models;
using Misc;
using Misc.Contracts;

namespace Application.User.Services;

public class UserService : IUserService
{
    private readonly PasswordService _passwordService;
    private readonly TokenService _tokenService;
    
    public UserService(PasswordService passwordService, TokenService tokenService)
    {
        _passwordService = passwordService;
        _tokenService = tokenService;
    }
    
    public Task<AuthResult> RegisterUser(string email, string password)
    {   
        var (hash, salt) = _passwordService.HashPassword(password);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var newUser = new UserModel(Guid.NewGuid(), email, hash, salt, refreshToken);
        var jwtToken = _tokenService.GenerateToken(new GenerateTokenRequest(newUser.Id.ToString(), newUser.Email));
        
        return Task.FromResult(new AuthResult(newUser, jwtToken, refreshToken));
    }

    public Task<AuthResult> LoginUser(string email, string password)
    {
        //TODO: password verification
        
        var refreshToken = _tokenService.GenerateRefreshToken();
        var user = new UserModel(Guid.NewGuid(), email, password, password, refreshToken);
        var jwtToken = _tokenService.GenerateToken(new GenerateTokenRequest(user.Id.ToString(), user.Email));
        
        return Task.FromResult(new AuthResult(user, jwtToken, refreshToken));
    }
}