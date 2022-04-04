namespace SiGameReborn.Common.Constants.ServiceUrls;

public static class UserProfileUrls
{
    public const string BaseUrl = "http://users/api/users";

    public static Uri ProfileDetailsUri(Guid userId) => new(Path.Combine(BaseUrl, userId.ToString(), "profile"));

    public static Uri ProfileDetailsUri(string usernameOrEmail) => new(Path.Combine(BaseUrl, usernameOrEmail, "profile"));
}