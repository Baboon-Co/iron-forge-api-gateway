using FluentResults;

namespace Application.Common.Errors;

public class ErrorInfoError(string reason, string domain) : Error(reason)
{
    public string Domain { get; } = domain;
}