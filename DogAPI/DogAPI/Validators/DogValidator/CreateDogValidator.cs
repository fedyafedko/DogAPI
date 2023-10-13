using Common.DTO.DogDTO;
using FluentValidation;

namespace DogAPI.Validators;

public class CreateDogValidator : AbstractValidator<CreateDogDTO>
{
    public CreateDogValidator()
    {
        RuleFor(d => d.Name).NotEmpty()
            .MinimumLength(1)
            .MaximumLength(50)
            .Matches(@"([A-Z]|[А-Я]){1}([a-z]|[а-я])*");

        RuleFor(d => d.Color).NotEmpty()
            .MinimumLength(1)
            .MaximumLength(50);
        
        RuleFor(d => d.TailLength).GreaterThan(0);

        RuleFor(d => d.Weight).GreaterThan(0);

    }
}