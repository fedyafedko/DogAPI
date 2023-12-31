﻿using Common.DTO.AuthDTO;
using FluentValidation;

namespace DogAPI.Validators.UserValidator;

public class RegisterUserValidator : AbstractValidator<RegisterUserDTO>
{
    public RegisterUserValidator()
    {
        RuleFor(u => u.Password).NotEmpty()
            .MinimumLength(1)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$");

        RuleFor(u => u.Login).NotEmpty()
            .MinimumLength(1);
    }
}