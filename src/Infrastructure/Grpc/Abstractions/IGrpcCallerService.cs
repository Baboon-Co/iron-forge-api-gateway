using FluentResults;

namespace Infrastructure.Grpc.Abstractions;

public interface IGrpcCallerService
{
    Task<Result<TResponse>> CallAsync<TRequest, TResponse>(
        Func<TRequest, Task<TResponse>> grpcCall,
        TRequest request,
        string operationName = "gRPC call")
        where TRequest : class
        where TResponse : class;
}