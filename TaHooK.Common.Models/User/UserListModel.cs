namespace TaHooK.Common.Models.User
{
    public record UserListModel : IWithId
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required Uri Photo { get; set; }
    }
}
