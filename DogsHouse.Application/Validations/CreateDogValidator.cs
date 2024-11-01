using DogsHouse.Domain.Models.RequestDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouse.Application.Validations
{
    public class CreateDogValidator : AbstractValidator<CreateDogRequestDto>
    {
        public CreateDogValidator()
        {
            RuleFor(dog => dog.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(dog => dog.Color)
                .NotEmpty().WithMessage("Color is required.")
                .MaximumLength(100).WithMessage("Color must not exceed 100 characters.");

            RuleFor(dog => dog.TailLength)
                .GreaterThanOrEqualTo(0).WithMessage("Tail length must be greater than or equal to 0");

            RuleFor(dog => dog.Weight)
                .GreaterThan(0).WithMessage("Weight must be greater than 0");
        }
    }
}
