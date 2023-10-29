using TaHooK.Common.Enums;

namespace TaHooK.Common.Models.Search;

public class SearchListItemModel : IWithId
{
    public required string Name { get; set; }
    public required SearchEntityType Type { get; set; }
    public Guid Id { get; set; }
}