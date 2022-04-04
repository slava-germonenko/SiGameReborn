using SiGameReborn.Registration.Core.Dtos;

namespace SiGameReborn.Registration.Core.Services;

public interface IRegistrationService
{
    Task RegisterUserAsync(RegistrationCredentials credentials);
}