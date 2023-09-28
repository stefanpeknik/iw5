namespace TaHooK.Common.Models.User
{
    public record UserListModel : IWithId
    {
        public Guid Id { get; init; }
    }
}
