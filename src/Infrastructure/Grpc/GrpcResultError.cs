using FluentResults;
using Grpc.Core;

namespace Infrastructure.Grpc;

public class GrpcResultError(string message = "Grpc error occurred") : Error(message)
{
    public StatusCode StatusCode { get; set; }
}