using Application.Common.Errors;
using FluentResults;
using Infrastructure.Grpc;

namespace Infrastructure.Extensions;

public static class FluentResultExtensions
{
    public static IEnumerable<ValidationError> GetValidationErrors<T>(this Result<T> result)
    {
        return result.Errors.OfType<ValidationError>();
    }

    public static GrpcResultError GetGrpcResultError<T>(this Result<T> result)
    {
        var grpcError = result.Errors.OfType<GrpcResultError>().FirstOrDefault();
        if (grpcError is null)
            throw new NullReferenceException("Required gRPC result error is null.");

        return grpcError;
    }
}