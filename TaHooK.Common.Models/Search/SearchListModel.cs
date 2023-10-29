namespace TaHooK.Common.Models.Search;

public record SearchListModel
{
    public int Page { get; set; }

    public int TotalPages { get; set; }

    public int TotalItems { get; set; }

    public IEnumerable<SearchListItemModel> Items { get; set; } = new List<SearchListItemModel>();
}