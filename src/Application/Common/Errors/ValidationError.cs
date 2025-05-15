using FluentResults;

namespace Application.Common.Errors;

public class ValidationError(string field, string message) : Error(message)
{
    public string Field { get; } = field;
}