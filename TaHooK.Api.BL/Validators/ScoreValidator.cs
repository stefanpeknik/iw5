using FluentValidation;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.Score;

namespace TaHooK.Api.BL.Validators;

public class ScoreValidator: AbstractValidator<ScoreDetailModel>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    
    public ScoreValidator(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;

        RuleFor(x => x.QuizId).Must(QuizExists).WithMessage("Quiz doesn't exist!");
        RuleFor(x => x.UserId).Must(UserExists).WithMessage("User doesn't exist!");
    }

    private bool QuizExists(Guid quizId)
    {
        var uow = _unitOfWorkFactory.Create();

        return uow.GetRepository<QuizEntity>().Exists(quizId);
    }
    private bool UserExists(Guid userId)
    {
        var uow = _unitOfWorkFactory.Create();

        return uow.GetRepository<UserEntity>().Exists(userId);
    }
}