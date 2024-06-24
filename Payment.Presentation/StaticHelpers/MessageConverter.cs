using FluentValidation.Results;

namespace Payment.Presentation.StaticHelpers;

public static class MessageConverter
{
    public static string ConvertValidationFailureListToMessage(IList<ValidationFailure> failures)
    {
        return string.Join("; ", failures.Select(failure =>
            $"{failure.PropertyName}: {failure.ErrorMessage}"));
    }
}