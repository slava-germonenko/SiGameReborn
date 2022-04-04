namespace SiGameReborn.Common.Constants.ServiceUrls;

public static class UserPasswordUrls
{
    public const string BaseUrl = "http://users/api/users";

    public static Uri UserPasswordsBaseUri(Guid userId) => new(
        Path.Combine(BaseUrl, userId.ToString(), "passwords")
    );
}