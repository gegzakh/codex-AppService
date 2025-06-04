using AppService.Models;
using Microsoft.AspNetCore.Identity;

namespace AppService.Services;

public class UserService : IUserService
{
    private readonly List<User> _users = new();
    private readonly PasswordHasher<User> _hasher = new();

    public Task<bool> UserExistsAsync(string userName)
    {
        return Task.FromResult(_users.Any(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)));
    }

    public async Task<User> RegisterAsync(string userName, string password)
    {
        if (await UserExistsAsync(userName))
        {
            throw new InvalidOperationException("User already exists");
        }

        var user = new User { UserName = userName };
        user.PasswordHash = _hasher.HashPassword(user, password);
        _users.Add(user);
        return user;
    }

    public Task<User?> LoginAsync(string userName, string password)
    {
        var user = _users.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        if (user == null) return Task.FromResult<User?>(null);
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return Task.FromResult(result == PasswordVerificationResult.Success ? user : null);
    }
}
