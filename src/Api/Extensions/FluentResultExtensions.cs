﻿using System.Net;
using BaboonCo.Utility.Grpc.Client.Errors;
using BaboonCo.Utility.Result.Extensions;
using BaboonCo.Utility.Result.ResultErrors.Enums;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

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

    public static ActionResult<T> ToErrorActionResult<T>(this Result<T> result)
    {
        var validationErrors = result.ToValidationErrorsDictionary();
        var requestError = result.GetRequestError();

        return requestError.Type switch
        {
            RequestErrorType.AlreadyExists => new ConflictObjectResult(
                new ValidationProblemDetails(validationErrors) {Detail = requestError.Message}),
            RequestErrorType.BadRequest => new BadRequestObjectResult(
                new ValidationProblemDetails(validationErrors)),
            RequestErrorType.NotFound => new NotFoundResult(),
            _ => new StatusCodeResult((int) HttpStatusCode.InternalServerError)
        };
    }
}