using Application.Abstractions;
using Grpc.Core;
using Polly;
using Polly.Retry;

namespace Infrastructure.Grpc;

using Microsoft.Extensions.Logging;

public class GrpcCallerService : IGrpcCallerService
{
    private readonly ILogger<GrpcCallerService> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;

    public GrpcCallerService(ILogger<GrpcCallerService> logger)
    {
        _logger = logger;

        _retryPolicy = Policy
            .Handle<RpcException>(ex => ex.StatusCode
                is StatusCode.Unavailable
                or StatusCode.DeadlineExceeded)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(300 * attempt),
                onRetry: (ex, ts, retryCount, _) =>
                {
                    logger.LogWarning(ex, "[gRPC] Retry {RetryCount} after {Delay}ms: {Message}",
                        retryCount, ts.TotalMilliseconds, ex.Message);
                });
    }

    public async Task<TResponse> CallAsync<TRequest, TResponse>(
        Func<TRequest, Task<TResponse>> grpcCall,
        TRequest request,
        string operationName = "gRPC call")
        where TRequest : class
        where TResponse : class
    {
        try
        {
            return await _retryPolicy.ExecuteAsync(() => grpcCall(request));
        }
        catch (RpcException ex)
        {
            _logger.LogError(ex, "[gRPC] {Operation} failed: {Status} {Detail}",
                operationName, ex.StatusCode, ex.Status.Detail);

            throw new ApplicationException($"[gRPC] {operationName} failed: {ex.Status.Detail}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[gRPC] {Operation} failed with unexpected error", operationName);
            throw;
        }
    }
}