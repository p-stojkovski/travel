using MediatR;
using System.Diagnostics;
using TravelInspiration.API.Shared.Metrics;

namespace TravelInspiration.API.Shared.Behaviours;

public sealed class HandlerPerformanceMetricBehaviour<TRequest, TResponse>(
    HandlerPerformanceMetric handlerPerformanceMetric)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly HandlerPerformanceMetric _handlerPerformanceMetric = handlerPerformanceMetric;
    private readonly Stopwatch _stopwatch = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        _stopwatch.Start();
        var response = await next();
        _stopwatch.Stop();

        _handlerPerformanceMetric.MiliSecondsElapsed(_stopwatch.ElapsedMilliseconds);

        return response;

    }
}
