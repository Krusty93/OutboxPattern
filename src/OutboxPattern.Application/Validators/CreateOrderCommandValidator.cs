using FluentValidation;
using OutboxPattern.Application.Commands;

namespace OutboxPattern.Application.Validators
{
    internal class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(cmd => cmd.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity is invalid");

            RuleFor(cmd => cmd.ShippingAddress)
                .NotNull()
                .SetValidator(new ShippingAddressDtoValidator());
        }
    }

    internal class ShippingAddressDtoValidator : AbstractValidator<ShippingAddressDto>
    {
        private const string NO_LEADING_MINUS_PATTERN = @"^(?![\s]*[-]+)";

        public ShippingAddressDtoValidator()
        {
            RuleFor(command => command.Street)
                .MinimumLength(3);

            RuleFor(command => command.Number)
                .GreaterThan(0)
                .WithMessage("Quantity is invalid");

            RuleFor(command => command.City)
                .NotEmpty()
                .WithMessage("Must provide FirstName.")
                .Matches(NO_LEADING_MINUS_PATTERN)
                .WithMessage("First name can't start with minus.");
        }
    }
}
