namespace SiGameReborn.Common.Domain.Models.Paging;

public record PageDescriptor
{
    public int Count { get; set; }

    public int Offset { get; set; }
}