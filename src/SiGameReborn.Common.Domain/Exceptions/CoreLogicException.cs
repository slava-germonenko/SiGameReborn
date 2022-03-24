namespace SiGameReborn.Common.Domain.Exceptions;

public class CoreLogicException : Exception
{
    public CoreLogicException(string? message) : base(message) { }

    public CoreLogicException(string? message, Exception? innerException) : base(message, innerException) { }
}