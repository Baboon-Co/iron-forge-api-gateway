using BaboonCo.Utility.Grpc.Client;
using FluentResults;
using Grpc.Core;
using Infrastructure.Grpc.Abstractions;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Infrastructure.Grpc;

public class GrpcCallerService : IGrpcCallerService
{
    private readonly ILogger<GrpcCallerService> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;

    public GrpcCallerService(ILogger<GrpcCallerService> logger)
    {
        _logger = logger;

        _retryPolicy = Policy
            .Handle<RpcException>(e => e.StatusCode
                is StatusCode.Unavailable
                or StatusCode.DeadlineExceeded)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(300 * attempt),
                onRetry: (e, ts, retryCount, _) =>
                {
                    logger.LogWarning(e, "[gRPC] Retry {RetryCount} after {Delay}ms: {Message}",
                        retryCount, ts.TotalMilliseconds, e.Message);
                });
    }

    public async Task<Result<TResponse>> CallAsync<TRequest, TResponse>(
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
        catch (RpcException e)
        {
            _logger.LogInformation(e, "[gRPC] {Operation} failed: {Status} {Detail}",
                operationName, e.StatusCode, e.Status.Detail);

            return GrpcClientHelper.ParseGrpcErrors(e);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[gRPC] {Operation} failed with unexpected error", operationName);
            throw;
        }
    }
}