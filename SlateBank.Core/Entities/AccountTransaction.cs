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
            RuleFor(at => at.AccountNumber).Must(dataStore.AccountNumberExists);
            RuleFor(at => at.AccountNumber).Must(dataStore.IsAccountActive)
                .WithMessage("Cannot do transaction with inactive account");
            RuleFor(at => at.Amount).GreaterThan(0m);
            
            RuleFor(at => new {at.AccountNumber, at.Amount}).Must(details =>
                    dataStore.IsDebitPossible(details.AccountNumber, details.Amount))
                .When(at => at.TransactionType == TransactionType.Debit && at.Amount > 0)
                .WithMessage("Insufficient Funds");
            
            RuleFor(at => at.Description).NotNull().MinimumLength(3);
        }
    }
}