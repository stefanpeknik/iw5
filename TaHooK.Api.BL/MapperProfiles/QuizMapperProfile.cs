using AutoMapper;
using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Extensions;
using TaHooK.Common.Models.Quiz;

namespace TaHooK.Api.BL.MapperProfiles;

public class QuizMapperProfile : Profile
{
    public QuizMapperProfile()
    {
        CreateMap<QuizEntity, QuizDetailModel>()
            .MapMember(dst => dst.Questions, src => src.Questions)
            .MapMember(dst => dst.Scores, src => src.Scores);
        
        CreateMap<QuizEntity, QuizListModel>();

        CreateMap<QuizDetailModel, QuizEntity>()
            .Ignore(dst => dst.Questions)
            .Ignore(dst => dst.Scores);
    }
}