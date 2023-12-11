using AutoMapper;
using TaHooK.Common.Extensions;

namespace TaHooK.Api.DAL.Entities;

public record QuizEntity : EntityBase
{
    public required string Title { get; set; }
    public required Guid TemplateId { get; set; }
    public QuizTemplateEntity? Template { get; set; }
    public required DateTime StartedAt { get; set; }
    public required bool Finished { get; set; }
    public ICollection<ScoreEntity> Scores { get; set; } = new List<ScoreEntity>();
    
    public Guid? CreatorId { get; set; }
    
    public UserEntity? Creator { get; set; }


    public class QuizEntityMapperProfile : Profile
    {
        public QuizEntityMapperProfile()
        {
            CreateMap<QuizEntity, QuizEntity>()
                .Ignore(dst => dst.Template);
        }
    }
}