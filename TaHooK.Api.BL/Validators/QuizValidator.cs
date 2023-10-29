using FluentValidation;
using TaHooK.Common.Models.Quiz;

namespace TaHooK.Api.BL.Validators;

public class QuizValidator: AbstractValidator<QuizCreateUpdateModel>
{
    public QuizValidator()
    {
    }
}