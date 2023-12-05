using AutoMapper;
using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Extensions;
using TaHooK.Common.Models.Quiz;
using TaHooK.Common.Models.Responses;

namespace TaHooK.Api.BL.MapperProfiles;

public class QuizMapperProfile : Profile
{
    public QuizMapperProfile()
    {
        CreateMap<QuizEntity, QuizDetailModel>()
            .MapMember(dst => dst.Scores, src => src.Scores);

        CreateMap<QuizEntity, QuizListModel>();

        CreateMap<QuizEntity, QuizCreateUpdateModel>();

        CreateMap<QuizCreateUpdateModel, QuizEntity>()
            .Ignore(dst => dst.Id)
            .Ignore(dst => dst.Scores);

        CreateMap<QuizEntity, IdModel>();
    }
}