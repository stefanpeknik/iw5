using AutoMapper;

namespace TaHooK.Api.DAL.Entities;

public record ScoreEntity : EntityBase
{
    public int Score { get; set; }

    public required Guid UserId { get; set; }
    public required UserEntity User { get; set; }

    public required Guid QuizId { get; set; }
    public required QuizEntity Quiz { get; set; }


    public class ScoreEntityMapperProfile : Profile
    {
        public ScoreEntityMapperProfile()
        {
            CreateMap<ScoreEntity, ScoreEntity>();
        }
    }
}