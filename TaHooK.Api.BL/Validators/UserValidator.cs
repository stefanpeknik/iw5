using FluentValidation;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.BL.Validators;

public class UserValidator: AbstractValidator<UserCreateUpdateModel>
{
    public UserValidator()
    {
    }
}