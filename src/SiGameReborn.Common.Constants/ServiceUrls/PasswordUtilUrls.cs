namespace SiGameReborn.Common.Constants.ServiceUrls;

public static class PasswordUtilUrls
{
    public const string BaseUrl = "http://users/api/passwords";

    public static Uri PasswordHashUri(string password)
    {
        var uriBuilder = new UriBuilder(Path.Combine(BaseUrl, "hash"))
        {
            Query =  $"password={Uri.EscapeDataString(password)}"
        };

        return uriBuilder.Uri;
    }
}