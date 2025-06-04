using AppService.Services;
using Xunit;

namespace AppService.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task Register_New_User_Succeeds()
    {
        var service = new UserService();
        var user = await service.RegisterAsync("alice", "password");
        Assert.Equal("alice", user.UserName);
        Assert.True(await service.UserExistsAsync("alice"));
    }

    [Fact]
    public async Task Register_Duplicate_User_Throws()
    {
        var service = new UserService();
        await service.RegisterAsync("alice", "password");
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegisterAsync("alice", "password"));
    }

    [Fact]
    public async Task Login_With_Correct_Credentials_Returns_User()
    {
        var service = new UserService();
        await service.RegisterAsync("alice", "password");
        var user = await service.LoginAsync("alice", "password");
        Assert.NotNull(user);
        Assert.Equal("alice", user!.UserName);
    }

    [Fact]
    public async Task Login_With_Wrong_Password_Returns_Null()
    {
        var service = new UserService();
        await service.RegisterAsync("alice", "password");
        var user = await service.LoginAsync("alice", "wrong");
        Assert.Null(user);
    }

    [Fact]
    public async Task Login_With_Unknown_User_Returns_Null()
    {
        var service = new UserService();
        var user = await service.LoginAsync("bob", "password");
        Assert.Null(user);
    }
}
