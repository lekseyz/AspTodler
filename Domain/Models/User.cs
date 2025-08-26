using Domain.ValueTypes;

namespace Domain.Models;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public Password Password { get; private set; }
    
    private User(Guid id, string email, Password password)
    {
        Id = id;
        Email = email;
        Password = password;
    }

    public static User Construct(Guid id, string email, Password password)
    {
        return new User(id, email, password);
    }
}