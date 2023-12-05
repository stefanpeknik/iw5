using AutoMapper;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Quiz;

namespace TaHooK.Api.BL.Facades;

public class QuizTemplateFacade : CrudFacadeBase<QuizTemplateEntity, QuizTemplateListModel, QuizTemplateDetailModel, QuizTemplateCreateUpdateModel>, IQuizTemplateFacade
{
    public QuizTemplateFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
    }

    public override List<string> NavigationPathDetails => new()
    {
        $"{nameof(QuizTemplateEntity.Questions)}"
    };
}