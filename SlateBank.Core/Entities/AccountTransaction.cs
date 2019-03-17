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
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Description { get; set; }
    }
    
    public class AccountTransactionValidator : AbstractValidator<AccountTransaction>
    {
        public AccountTransactionValidator()
        {
            RuleFor(at => at.Amount).GreaterThanOrEqualTo(0m);
            RuleFor(at => at.Description).NotNull().MinimumLength(3);
        }
    }
}