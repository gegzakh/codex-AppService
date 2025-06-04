namespace AppService.Models;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
