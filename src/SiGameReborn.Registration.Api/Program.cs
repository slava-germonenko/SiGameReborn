using Microsoft.AspNetCore.Mvc;

using SiGameReborn.Common.Domain.Exceptions;
using SiGameReborn.Common.Web.Responses;
using SiGameReborn.Registration.Application.Services;
using SiGameReborn.Registration.Core.Dtos;
using SiGameReborn.Registration.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

var app = builder.Build();

app.MapPost(
    "api/registration",
    async (
        [FromServices] IRegistrationService registrationService,
        [FromBody] RegistrationCredentials credentials
    ) =>
    {
        try
        {
            await registrationService.RegisterUserAsync(credentials);
        }
        catch (DuplicateException ex)
        {
            return Results.BadRequest(new BaseErrorResponse(ex.Message));
        }
        catch (CoreLogicException ex)
        {
            return Results.BadRequest(new BaseErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            var result = app.Environment.IsDevelopment()
                ? new DeveloperErrorErrorResponse(ex.Message, ex.StackTrace)
                : new BaseErrorResponse(ex.Message);

            return Results.BadRequest(result);
        }
        return Results.NoContent();
    }
);

app.Run();