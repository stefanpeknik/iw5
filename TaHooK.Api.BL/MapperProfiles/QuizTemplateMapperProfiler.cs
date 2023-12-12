using AutoMapper;
using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Extensions;
using TaHooK.Common.Models.Quiz;
using TaHooK.Common.Models.Responses;

namespace TaHooK.Api.BL.MapperProfiles;

public class QuizTemplateMapperProfile : Profile
{
    public QuizTemplateMapperProfile()
    {
        CreateMap<QuizTemplateEntity, QuizTemplateDetailModel>()
            .MapMember(dst => dst.Questions, src => src.Questions)
            .MapMember(dst => dst.CreatorName, src => src.Creator!.Name);

        CreateMap<QuizTemplateEntity, QuizTemplateListModel>()
            .MapMember(dst => dst.CreatorName, src => src.Creator!.Name);

        CreateMap<QuizTemplateEntity, QuizTemplateCreateUpdateModel>();

        CreateMap<QuizTemplateCreateUpdateModel, QuizTemplateEntity>()
            .Ignore(dst => dst.Id)
            .Ignore(dst => dst.Questions)
            .Ignore(dst => dst.Creator);

        CreateMap<QuizTemplateEntity, IdModel>();
    }
}