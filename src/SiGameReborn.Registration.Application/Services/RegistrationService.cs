using System.Net.Http.Json;
using System.Text.Json;
using SiGameReborn.Common.Constants.ServiceUrls;
using SiGameReborn.Common.Domain.Exceptions;
using SiGameReborn.Common.Domain.Models;
using SiGameReborn.Common.Web.Responses;
using SiGameReborn.Registration.Core.Dtos;
using SiGameReborn.Registration.Core.Services;

namespace SiGameReborn.Registration.Application.Services;

public class RegistrationService : IRegistrationService
{
    private readonly HttpClient _httpClient;

    public RegistrationService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("registration");
    }

    public async Task RegisterUserAsync(RegistrationCredentials credentials)
    {
        var userId = await CreateUserAsync(credentials);
        var passwordHash = await GeneratePasswordHashAsync(credentials.Password);
        await SaveUserPassword(userId, passwordHash);
    }

    private async Task<Guid> CreateUserAsync(RegistrationCredentials credentials)
    {
        var registeredUserResponse = await _httpClient.PostAsJsonAsync(UserProfileUrls.BaseUrl, credentials);
        if (!registeredUserResponse.IsSuccessStatusCode)
        {
            var error = await registeredUserResponse.Content.ReadFromJsonAsync<BaseErrorResponse>();
            throw new DuplicateException(error?.Message);
        }

        var user = await registeredUserResponse.Content.ReadFromJsonAsync<BaseEntity>();
        return user!.Id;
    }

    private async Task<string> GeneratePasswordHashAsync(string password)
    {
        return await _httpClient.GetStringAsync(PasswordUtilUrls.PasswordHashUri(password));
    }

    private async Task SaveUserPassword(Guid userId, string passwordHash)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync(
            UserPasswordUrls.UserPasswordsBaseUri(userId),
            new { passwordHash }
        );

        if (!responseMessage.IsSuccessStatusCode)
        {
            var error = await responseMessage.Content.ReadFromJsonAsync<BaseErrorResponse>();
            throw new CoreLogicException(error?.Message);
        }
    }
}