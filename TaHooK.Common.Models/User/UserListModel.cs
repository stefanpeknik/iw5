namespace TaHooK.Common.Models.User;

public record UserListModel : IWithId
{
    public required string Name { get; set; }
    public required Uri Photo { get; set; }
    public Guid Id { get; set; }
}