using DogsHouse.Domain.Enums;
using DogsHouse.Domain.Models.RequestDtos;
using FluentValidation;

namespace DogsHouse.Application.Validations
{
    public class DogQueryValidator : AbstractValidator<GetDogsRequestDto>
    {
        public DogQueryValidator()
        {
            RuleFor(query => query.Attribute)
            .NotEmpty().WithMessage("Attribute is required.")
            .Must(BeAValidAttribute).WithMessage("Invalid attribute.");

            RuleFor(query => query.Order)
                .NotEmpty().WithMessage("Order is required.")
                .Must(BeAValidOrder).WithMessage("Order must be 'asc' or 'desc'.");

            RuleFor(query => query.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(query => query.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.");
        }

        private bool BeAValidAttribute(string attribute)
        {
            return Enum.TryParse<DogQueryAttributes>(attribute, true, out _);
        }

        private bool BeAValidOrder(string order)
        {
            return Enum.TryParse<SortOrder>(order, true, out _);
        }
    }
}
