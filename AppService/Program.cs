var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<AppService.Services.IUserService, AppService.Services.UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/register", async (AppService.Models.RegisterRequest req, AppService.Services.IUserService userService) =>
{
    if (await userService.UserExistsAsync(req.UserName))
    {
        return Results.BadRequest("User already exists");
    }
    var user = await userService.RegisterAsync(req.UserName, req.Password);
    return Results.Ok(new { user.Id, user.UserName });
}).WithName("Register");

app.MapPost("/login", async (AppService.Models.LoginRequest req, AppService.Services.IUserService userService) =>
{
    var user = await userService.LoginAsync(req.UserName, req.Password);
    if (user is null)
    {
        return Results.Unauthorized();
    }
    return Results.Ok(new { user.Id, user.UserName });
}).WithName("Login");

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
