using System.Collections.Generic;
using FluentValidation;

namespace SlateBank.Core.Entities
{
    public class Account
    {
        public string CustomerID { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal OverdraftLimit { get; set; }
        public bool IsActive { get; set; }
        
        public IList<AccountTransaction> Transactions { get; set; }
    }
    
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(c => c.CustomerID).Length(8);
            RuleFor(c => c.AccountNumber).Length(8);
            RuleFor(c => c.OverdraftLimit).InclusiveBetween(0, 1000);
        }
    }
}