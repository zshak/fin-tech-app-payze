using FluentValidation;
using Payment.Presentation.DTOs;

namespace Payment.Presentation.Validator;

public class PayForOrderValidator : AbstractValidator<PayRequest>
{
    public PayForOrderValidator()
    {
        RuleFor(card => card.CardNumber)
            .NotEmpty().WithMessage("Card number is required.")
            .Length(13, 19).WithMessage("Card number must be between 13 and 19 digits.")
            .Matches(@"^\d+$").WithMessage("Card number must be numeric.");
    }
}