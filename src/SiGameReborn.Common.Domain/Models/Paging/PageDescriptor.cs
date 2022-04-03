namespace SiGameReborn.Common.Domain.Models.Paging;

public record PageDescriptor
{
    public const int DefaultCount = 50;

    public const int DefaultOffset = 0;

    public int Count { get; set; } = DefaultCount;

    public int Offset { get; set; } = DefaultOffset;
}