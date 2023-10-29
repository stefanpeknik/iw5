using AutoMapper;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Question;

namespace TaHooK.Api.BL.Facades;

public class QuestionFacade :
    CrudFacadeBase<QuestionEntity, QuestionListModel, QuestionDetailModel, QuestionCreateUpdateModel>, IQuestionFacade
{
    public QuestionFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
    }

    public override List<string> NavigationPathDetails => new()
    {
        $"{nameof(QuestionEntity.Answers)}"
    };
}