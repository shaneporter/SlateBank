using System;
using FluentValidation;

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
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Description { get; set; }
    }
    
    public class AccountTransactionValidator : AbstractValidator<AccountTransaction>
    {
        public AccountTransactionValidator(IDataStore dataStore)
        {
            RuleFor(at => at.Amount).GreaterThanOrEqualTo(0m);
            RuleFor(at => at.Description).NotNull().MinimumLength(3);
            RuleFor(at => at.AccountNumber).Must(dataStore.AccountNumberExists);
        }
    }
}