namespace SiGameReborn.User.Api.Options;

public class Passwords
{
    public int HashLength { get; set; }

    public string HashAlgorithmName { get; set; } = "SHA256";
}