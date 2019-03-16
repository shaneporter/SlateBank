using System;

namespace SlateBank.Core.Entities
{
    public enum TransactionType
    {
        Debit,
        Credit,
        Transfer
    };
    
    public class AccountTransaction
    {
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
    }
}