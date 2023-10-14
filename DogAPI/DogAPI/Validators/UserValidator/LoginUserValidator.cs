using Common.DTO.AuthDTO;
using FluentValidation;

namespace DogAPI.Validators.UserValidator;

public class LoginUserValidator : AbstractValidator<LoginUserDTO>
{
    public LoginUserValidator()
    {
        RuleFor(u => u.Password).NotEmpty()
            .MinimumLength(1);

        RuleFor(u => u.Login).NotEmpty()
            .MinimumLength(1);
    }
}