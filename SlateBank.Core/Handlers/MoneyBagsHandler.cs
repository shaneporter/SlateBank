using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SlateBank.Core.Events;

namespace SlateBank.Core.Handlers
{
    public class MoneyBagsHandler
    {
        private readonly ILogger<GoneOverdrawnHandler> _logger;

        public MoneyBagsHandler(ILogger<GoneOverdrawnHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(BalanceEvent notification, CancellationToken cancellationToken)
        {
            return Task.Run(() => _logger.LogWarning(
                    $"WEALTH ALERT: Customer: {notification.CustomerID}, Account: {notification.AccountNumber}, Balance: {notification.Balance}"),
                cancellationToken);
        }
    }
}