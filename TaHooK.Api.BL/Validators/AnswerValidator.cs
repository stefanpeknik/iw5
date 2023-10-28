using FluentValidation;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Answer;

namespace TaHooK.Api.BL.Validators;

public class AnswerValidator: AbstractValidator<AnswerDetailModel>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    
    public AnswerValidator(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;

        RuleFor(x => x.QuestionId).Must(QuestionExists).WithMessage("Question doesn't exist!");
    }

    private bool QuestionExists(Guid questionId)
    {
        var uow = _unitOfWorkFactory.Create();

        return uow.GetRepository<QuestionEntity>().Exists(questionId);
    }
}