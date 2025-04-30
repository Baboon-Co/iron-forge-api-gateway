namespace Application.Abstractions;

public interface IGrpcCallerService
{
    Task<TResponse> CallAsync<TRequest, TResponse>(
        Func<TRequest, Task<TResponse>> grpcCall,
        TRequest request,
        string operationName = "gRPC call")
        where TRequest : class
        where TResponse : class;
}