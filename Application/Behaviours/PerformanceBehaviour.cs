using Application.Interfaces.Services.CurrentUserService;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;

        public PerformanceBehaviour(
            ILogger<TRequest> logger,
            ICurrentUserService currentUserService
           )
        {
            _timer = new Stopwatch();
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var totalSeconds = _timer.Elapsed.TotalSeconds;
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? string.Empty;
            var userName = string.Empty;
            if (totalSeconds > 5)
            {
                _logger.LogWarning("Mediator Performance Long Running Request: {Name} ({totalSeconds} second) {@UserId}  {@Request}",
                    requestName, totalSeconds, userId, request);
            }
            else
            {
                _logger.LogInformation("Mediator Performance Long Running Request: {Name} ({totalSeconds} second) {@UserId}  {@Request}",
                   requestName, totalSeconds, userId, request);
            }

            return response;
        }
    }
}
