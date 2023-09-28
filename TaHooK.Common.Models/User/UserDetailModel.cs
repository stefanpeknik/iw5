namespace TaHooK.Common.Models.User
{
    public record UserDetailModel : IWithId
    {
        public Guid Id { get; init; }
    }
}
