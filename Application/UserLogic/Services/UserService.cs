using Application.UserLogic.Contracts;
using Application.UserLogic.Interfaces;
using Application.Common.ErrorTypes;
using Application.Common;
using Domain.Models;
using Domain.ValueTypes;
using Persistence.Repositories;
using Infrastructure;

namespace Application.UserLogic.Services;

public class UserService : IUserService
{
    private readonly PasswordService _passwordService;
    private readonly TokenService _tokenService;
    private readonly UserRepository _userRepository;
    private readonly RefreshTokenRepository _refreshTokenRepository;
    
    public UserService(PasswordService passwordService, TokenService tokenService, UserRepository userRepository, RefreshTokenRepository refreshTokenRepository)
    {
        _passwordService = passwordService;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<List<User>> GetUsers() //TODO: pagination
    {
        return await _userRepository.GetAll();
    }

    public async Task<ResultOptional<User>> Get(Guid id)
    {
        var user = await _userRepository.GetById(id);
        if  (user == null)
            return ResultOptional<User>.Failure(new NotFoundError($"User with id {id} not found."));
        
        return ResultOptional<User>.Success(user!);
    }
    
    public async Task<ResultOptional<AuthResult>> RegisterUser(string email, string password)
    {   
        if (await _userRepository.IsEmailPresent(email)) 
            return ResultOptional<AuthResult>.Failure(new InputError($"Email {email} already exists.")); 
        
        var (hash, salt) = _passwordService.HashPassword(password);
        var pass = new Password(hash, salt);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var newUser = User.Construct(Guid.NewGuid(), email, pass);
        
        var jwtToken = _tokenService.GenerateToken(newUser);
        
        await _userRepository.Add(newUser);
        await _refreshTokenRepository.Add(newUser.Id, refreshToken);
        
        var authResult = new AuthResult(newUser, jwtToken, refreshToken.Token);
        return ResultOptional<AuthResult>.Success(authResult);
    }

    public async Task<ResultOptional<AuthResult>> LoginUser(string email, string password)
    {
        var user= await _userRepository.GetByEmail(email);
        
        if (user is null)
            return ResultOptional<AuthResult>.Failure(new AuthError("Cannot login error"));

        var pass = user.Password;
        if (!_passwordService.VerifyPassword(pass.Hash, password, pass.Salt)) 
            return ResultOptional<AuthResult>.Failure(new AuthError("Cannot login error")); 
        
        var refreshToken = _tokenService.GenerateRefreshToken();
        var jwtToken = _tokenService.GenerateToken(user);
        
        await _refreshTokenRepository.Add(user.Id, refreshToken);
        
        var authResult = new AuthResult(user, jwtToken, refreshToken.Token);
        return ResultOptional<AuthResult>.Success(authResult);
    }

    public async Task<ResultOptional<AuthResult>> RefreshToken(Guid userId, string refreshToken)
    {
        var user = await _userRepository.GetById(userId);
        if (user is null)
            return ResultOptional<AuthResult>.Failure(new NotFoundError($"User {userId} not found"));
        
        if (!await _refreshTokenRepository.Check(userId, refreshToken))
            return ResultOptional<AuthResult>.Failure(new AuthError("Refresh token is invalid"));

        var newToken = _tokenService.GenerateRefreshToken();
        var jwtToken = _tokenService.GenerateToken(user!);
        await _refreshTokenRepository.Update(userId, refreshToken, newToken);
        
        return ResultOptional<AuthResult>.Success(new AuthResult(user!, jwtToken, newToken.Token));
    }
}