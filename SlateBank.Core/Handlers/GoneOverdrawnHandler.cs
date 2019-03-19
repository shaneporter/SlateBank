using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SlateBank.Core.Events;

namespace SlateBank.Core.Handlers
{    
    public class GoneOverdrawnHandler : INotificationHandler<BalanceEvent>
    {
        private readonly ILogger<GoneOverdrawnHandler> _logger;

        public GoneOverdrawnHandler(ILogger<GoneOverdrawnHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(BalanceEvent notification, CancellationToken cancellationToken)
        {
            return Task.Run(() => _logger.LogWarning(
                $"OVERDRAWN: Customer: {notification.CustomerID}, Account: {notification.AccountNumber}, Balance: {notification.Balance}"),
                cancellationToken);
        }
    }
}