using FluentResults;

namespace Application.Errors;

public class ErrorInfoError(string reason, string domain) : Error(reason)
{
    public string Domain { get; } = domain;
}