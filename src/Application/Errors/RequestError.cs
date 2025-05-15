using System.Net;
using FluentResults;

namespace Application.Errors;

public class RequestError(string reason, HttpStatusCode statusCode) : Error(reason)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}