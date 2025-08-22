using Application.User.Contracts;
using Domain.Models;

namespace Application.User.Interfaces;

public interface IUserService
{
    public Task<AuthResult> RegisterUser(string email, string password);
    public Task<AuthResult> LoginUser(string email, string password);
}