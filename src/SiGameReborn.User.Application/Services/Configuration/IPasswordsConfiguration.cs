using System.Security.Cryptography;

namespace SiGameReborn.User.Application.Services.Configuration;

public interface IPasswordsConfiguration
{
    public HashAlgorithm CreateHashAlgorithm();
}