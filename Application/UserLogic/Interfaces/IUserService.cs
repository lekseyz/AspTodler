using Application.Common;
using Application.UserLogic.Contracts;
using Domain.Models;

namespace Application.UserLogic.Interfaces;

public interface IUserService
{
    public Task<ResultOptional<User>> Get(Guid userId);
    public Task<List<User>> GetUsers();
    public Task<ResultOptional<AuthResult>> RegisterUser(string email, string password);
    public Task<ResultOptional<AuthResult>> LoginUser(string email, string password);
    public Task<ResultOptional<AuthResult>> RefreshToken(Guid userId, string refreshToken);
}