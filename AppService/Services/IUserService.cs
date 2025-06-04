using AppService.Models;

namespace AppService.Services;

public interface IUserService
{
    Task<User> RegisterAsync(string userName, string password);
    Task<User?> LoginAsync(string userName, string password);
    Task<bool> UserExistsAsync(string userName);
}
