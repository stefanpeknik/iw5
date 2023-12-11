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
            .MapMember(dst => dst.Scores, src => src.Scores)
            .MapMember(dst => dst.Questions, src => src.Template!.Questions)
            .MapMember(dst => dst.CreatorName, src => src.Creator!.Name);

        CreateMap<QuizEntity, QuizListModel>()
            .MapMember(dst => dst.CreatorName, src => src.Creator!.Name);

        CreateMap<QuizEntity, QuizCreateUpdateModel>();

        CreateMap<QuizCreateUpdateModel, QuizEntity>()
            .Ignore(dst => dst.Id)
            .Ignore(dst => dst.Template)
            .Ignore(dst => dst.Scores)
            .Ignore(dst => dst.Creator);

        CreateMap<QuizEntity, IdModel>();
    }
}