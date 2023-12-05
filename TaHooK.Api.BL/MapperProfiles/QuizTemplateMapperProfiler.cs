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
            .MapMember(dst => dst.Questions, src => src.Questions);

        CreateMap<QuizTemplateEntity, QuizTemplateListModel>();

        CreateMap<QuizTemplateEntity, QuizTemplateCreateUpdateModel>();

        CreateMap<QuizTemplateCreateUpdateModel, QuizTemplateEntity>()
            .Ignore(dst => dst.Id)
            .Ignore(dst => dst.Questions);

        CreateMap<QuizTemplateEntity, IdModel>();
    }
}