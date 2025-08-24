using Application.User.Contracts;
using Application.User.Interfaces;
using Domain.Models;
using Infrastructure.Repositories;
using Misc;

namespace Application.User.Services;

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

    public async Task<List<UserModel>> GetAll() //TODO: pagination
    {
        var users = await _userRepository.GetAll();
        return users.Select(u => new UserModel(u.Guid, u.Email, u.PasswordHash, u.PasswordSalt)).ToList();
    }

    public async Task<ResultOptional<UserModel>> Get(Guid id)
    {
        var userResult = await _userRepository.GetById(id);
        
        if (userResult.IsFailure)
            return  ResultOptional<UserModel>.Failure(userResult.GetError);
        
        var user = userResult.Value;
        return ResultOptional<UserModel>.Success(user!);
    }
    
    public async Task<ResultOptional<AuthResult>> RegisterUser(string email, string password)
    {   
        if (await _userRepository.IsEmailPresent(email)) 
            return ResultOptional<AuthResult>.Failure(Error.WrongValue($"Email {email} is already in use.")); 
        
        var (hash, salt) = _passwordService.HashPassword(password);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var newUser = new UserModel(Guid.NewGuid(), email, hash, salt);
        
        var jwtToken = _tokenService.GenerateToken(newUser);
        
        await _userRepository.Add(newUser);
        await _refreshTokenRepository.Add(newUser.Id, refreshToken);
        
        var authResult = new AuthResult(newUser, jwtToken, refreshToken.Token);
        return ResultOptional<AuthResult>.Success(authResult);
    }

    public async Task<ResultOptional<AuthResult>> LoginUser(string email, string password)
    {
        var userResult = await _userRepository.GetByEmail(email);
        
        if (userResult.IsFailure)
            return ResultOptional<AuthResult>.Failure(Error.AuthError("Wrong email or password."));
        var user =  userResult.Value;
        
        if (!_passwordService.VerifyPassword(user!.PasswordHash, password, user.PasswordSalt)) 
            return ResultOptional<AuthResult>.Failure(Error.AuthError("Wrong email or password.")); 
        
        var refreshToken = _tokenService.GenerateRefreshToken();
        var jwtToken = _tokenService.GenerateToken(user);
        
        await _refreshTokenRepository.Add(user.Id, refreshToken);
        
        var authResult = new AuthResult(user, jwtToken, refreshToken.Token);
        return ResultOptional<AuthResult>.Success(authResult);
    }

    public async Task<ResultOptional<AuthResult>> RefreshToken(Guid userId, string refreshToken)
    {
        if (!await _refreshTokenRepository.Check(userId, refreshToken))
            return ResultOptional<AuthResult>.Failure(Error.AuthError("Wrong refresh token."));

        var userResult = await _userRepository.GetById(userId);
        if (userResult.IsFailure)
            return ResultOptional<AuthResult>.Failure(userResult.GetError);

        var user = userResult.Value;
        var newToken = _tokenService.GenerateRefreshToken();
        var jwtToken = _tokenService.GenerateToken(user!);
        var refreshTokenResult = await _refreshTokenRepository.Update(userId, refreshToken, newToken);
        
        if (refreshTokenResult.IsFailure)
            return ResultOptional<AuthResult>.Failure(refreshTokenResult.GetError);
        return ResultOptional<AuthResult>.Success(new AuthResult(user!, jwtToken, newToken.Token));
    }
}