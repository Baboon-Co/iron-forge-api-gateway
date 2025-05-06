using FluentResults;

namespace Application.Errors;

public class ValidationError(string field, string message) : Error(message)
{
    public string Field { get; set; } = field;
}