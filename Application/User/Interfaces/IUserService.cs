using Application.User.Contracts;
using Domain.Models;
using Misc;

namespace Application.User.Interfaces;

public interface IUserService
{
    public Task<ResultOptional<UserModel>> Get(Guid userId);
    public Task<List<UserModel>> GetAll();
    public Task<ResultOptional<AuthResult>> RegisterUser(string email, string password);
    public Task<ResultOptional<AuthResult>> LoginUser(string email, string password);
    public Task<ResultOptional<AuthResult>> RefreshToken(Guid userId, string refreshToken);
}