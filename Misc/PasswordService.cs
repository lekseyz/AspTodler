using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Misc;

public class PasswordService
{
    /// <summary>
    /// Method for generating hash of password and salt
    /// </summary>
    /// <param name="password">String password argument</param>
    /// <returns>(string hash, base64 string salt)</returns>
    public (string, string) HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        
        var saltString = Convert.ToBase64String(salt);
        
        return (hashed, saltString);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var newHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: providedPassword,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
            ));
        
        return hashedPassword.Equals(newHash);
    }
}