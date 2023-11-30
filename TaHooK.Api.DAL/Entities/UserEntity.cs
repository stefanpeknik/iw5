using AutoMapper;

namespace TaHooK.Api.DAL.Entities;

public record UserEntity : EntityBase
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required Uri Photo { get; set; }

    public ICollection<ScoreEntity> Scores { get; set; } = new List<ScoreEntity>();


    public class UserEntityMapperProfile : Profile
    {
        public UserEntityMapperProfile()
        {
            CreateMap<UserEntity, UserEntity>();
        }
    }
}