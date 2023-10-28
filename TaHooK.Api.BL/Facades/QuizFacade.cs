using AutoMapper;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Quiz;

namespace TaHooK.Api.BL.Facades;

public class QuizFacade: FacadeBase<QuizEntity, QuizListModel, QuizDetailModel>, IQuizFacade
{
    public QuizFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
    }
    
    public override List<string> NavigationPathDetails => new()
    {
        $"{nameof(QuizEntity.Questions)}",
        $"{nameof(QuizEntity.Scores)}"
    };
}