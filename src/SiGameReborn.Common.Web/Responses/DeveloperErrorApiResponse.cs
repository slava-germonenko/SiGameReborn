namespace SiGameReborn.Common.Web.Responses;

public record DeveloperErrorApiResponse(string Message, string? Stacktrace)
    : BaseApiResponse(Message);