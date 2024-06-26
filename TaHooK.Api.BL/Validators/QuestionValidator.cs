using FluentValidation;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Question;

namespace TaHooK.Api.BL.Validators;

public class QuestionValidator : AbstractValidator<QuestionCreateUpdateModel>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public QuestionValidator(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;

        RuleFor(x => x.QuizTemplateId).Must(QuizExists).WithMessage(x => $"Quiz with Id={x.QuizTemplateId} doesn't exist!");
    }

    private bool QuizExists(Guid quizId)
    {
        var uow = _unitOfWorkFactory.Create();

        return uow.GetRepository<QuizTemplateEntity>().Exists(quizId);
    }
}