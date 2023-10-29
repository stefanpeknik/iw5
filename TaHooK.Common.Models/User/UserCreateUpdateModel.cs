namespace TaHooK.Common.Models.User;

public record UserCreateUpdateModel
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required Uri Photo { get; set; }
};