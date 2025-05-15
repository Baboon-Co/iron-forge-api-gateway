using Application.Errors;
using FluentResults;

namespace Api.Extensions;

public static class FluentResultExtensions
{
    public static Dictionary<string, string[]> ToValidationErrorsDictionary<T>(this Result<T> result)
    {
        return result.Errors
            .OfType<ValidationError>()
            .GroupBy(e => e.Field)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Message).ToArray()
            );
    }
    
    public static RequestError GetRequestError<T>(this Result<T> result)
    {
        return result.Errors.OfType<RequestError>().First();
    }
}