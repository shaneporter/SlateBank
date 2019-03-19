using MediatR;

namespace SlateBank.Core.Events
{
    public class BalanceEvent : INotification {
        
        public BalanceEvent(string customerID, string accountNumber, decimal balance)
        {
            CustomerID = customerID;
            AccountNumber = accountNumber;
            Balance = balance;
        }

        public string CustomerID { get; }
        
        public string AccountNumber { get; }
        public decimal Balance { get; }
    }
}