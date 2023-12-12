using AutoMapper;
using TaHooK.Common.Extensions;

namespace TaHooK.Api.DAL.Entities;

public record ScoreEntity : EntityBase
{
    public int Score { get; set; }

    public required Guid UserId { get; set; }
    public UserEntity? User { get; set; }

    public required Guid QuizId { get; set; }
    public QuizEntity? Quiz { get; set; }


    public class ScoreEntityMapperProfile : Profile
    {
        public ScoreEntityMapperProfile()
        {
            CreateMap<ScoreEntity, ScoreEntity>()
                .Ignore(dst => dst.Quiz)
                .Ignore(dst => dst.User);
        }
    }
}