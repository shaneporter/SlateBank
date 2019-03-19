using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SlateBankApi
{
    public class SomeEvent : INotification {
        public SomeEvent(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    public class Handler1 : INotificationHandler<SomeEvent>
    {
        private readonly ILogger<Handler1> _logger;

        public Handler1(ILogger<Handler1> logger)
        {
            _logger = logger;
        }

        public Task Handle(SomeEvent notification, CancellationToken cancellationToken)
        {
            return Task.Run(() => _logger.LogWarning(notification.Message), cancellationToken);
        }
    }
    
    public class Handler2 : INotificationHandler<SomeEvent>
    {
        private readonly ILogger<Handler2> _logger;

        public Handler2(ILogger<Handler2> logger)
        {
            _logger = logger;
        }

        public Task Handle(SomeEvent notification, CancellationToken cancellationToken)
        {
            return Task.Run(() => _logger.LogInformation(notification.Message), cancellationToken);
        }
    }
}