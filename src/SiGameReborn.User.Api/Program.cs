using Microsoft.EntityFrameworkCore;

using SiGameReborn.User.Api.Middleware;
using SiGameReborn.User.Application.Services;
using SiGameReborn.User.Core;
using SiGameReborn.User.Core.Services;

var builder = WebApplication.CreateBuilder(args);

var appConfigurationConnectionString = builder.Configuration.GetValue<string>("AppConfigurationConnectionString");

if (!string.IsNullOrEmpty(appConfigurationConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(appConfigurationConnectionString);
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IUsersListService, UsersListService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserPasswordService, UserPasswordService>();

var coreSqlConnectionString = builder.Configuration.GetValue<string>("CoreSqlConnectionString");
builder.Services.AddDbContext<UserContext>(config =>
{
    config.UseSqlServer(coreSqlConnectionString);
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();