using TaHooK.Common.Models.Score;

namespace TaHooK.Common.Models.User
{
    public record UserDetailModel : IWithId
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required Uri Photo { get; set; }

        public ICollection<ScoreListModel> Scores { get; set; } = new List<ScoreListModel>();
    }
}
