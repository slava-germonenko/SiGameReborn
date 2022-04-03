using Microsoft.EntityFrameworkCore;
using SiGameReborn.Tokens.Api.Configuration;
using SiGameReborn.Tokens.Api.Settings;
using SiGameReborn.Tokens.Application.Configuration;
using SiGameReborn.Tokens.Application.Services;
using SiGameReborn.Tokens.Core;
using SiGameReborn.Tokens.Core.Services;

var builder = WebApplication.CreateBuilder(args);

var appConfigurationConnectionString = builder.Configuration.GetValue<string>("AppConfigurationConnectionString");

if (!string.IsNullOrEmpty(appConfigurationConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(appConfigurationConnectionString);
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<Security>(builder.Configuration.GetSection(nameof(Security)));
builder.Services.AddScoped<IRefreshTokensConfiguration, OptionsRefreshTokensConfiguration>();
builder.Services.AddScoped<IRefreshTokensListService, RefreshTokensListService>();
builder.Services.AddScoped<IRefreshTokensService, RefreshTokensService>();

var coreSqlConnectionString = builder.Configuration.GetValue<string>("CoreSqlConnectionString");
builder.Services.AddDbContext<TokensContext>(config =>
{
    config.UseSqlServer(coreSqlConnectionString);
});

var app = builder.Build();

app.MapControllers();

app.Run();