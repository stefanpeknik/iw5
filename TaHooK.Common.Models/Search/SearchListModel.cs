using TaHooK.Common.Enums;

namespace TaHooK.Common.Models.Search;

public record SearchListModel : IWithId
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required SearchEntityType Type { get; set; }
    
}