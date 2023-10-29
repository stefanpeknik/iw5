namespace TaHooK.Common.Models.Responses;

public record IdModel : IWithId
{
    public required Guid Id { get; set; }
}