using CleanArchitecture.Shared.Models.User;
using FluentValidation;

namespace CleanArchitecture.Web.Validations;
public class UserSignInRequestValidation : AbstractValidator<UserSignInRequest>
{
    public UserSignInRequestValidation()
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(100);
    }
}
