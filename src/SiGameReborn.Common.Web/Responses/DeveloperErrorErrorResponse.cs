namespace SiGameReborn.Common.Web.Responses;

public record DeveloperErrorErrorResponse(string Message, string? Stacktrace)
    : BaseErrorResponse(Message);