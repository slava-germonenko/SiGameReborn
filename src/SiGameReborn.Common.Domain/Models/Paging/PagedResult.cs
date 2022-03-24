namespace SiGameReborn.Common.Domain.Models.Paging;

public record PagedResult<TItem>
{
    public int Total { get; set; }

    public PageDescriptor Page { get; set; }

    public IList<TItem> Items { get; set; } = new List<TItem>();
}