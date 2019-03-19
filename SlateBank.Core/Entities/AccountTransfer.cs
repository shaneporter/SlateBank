using FluentValidation;

namespace SlateBank.Core.Entities
{
    public class AccountTransfer
    {
        public decimal Amount { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public string Description { get; set; }
    }

    public class AccountTransferValidator : AbstractValidator<AccountTransfer>
    {
        public AccountTransferValidator(IDataStore dataStore)
        {
            RuleFor(at => at.Amount).GreaterThan(0m);
            RuleFor(at => at.FromAccount).Must(dataStore.AccountNumberExists).WithMessage("'From Account' not found");
            RuleFor(at => at.ToAccount).Must(dataStore.AccountNumberExists).WithMessage("'To Account' not found");
            RuleFor(at => new {at.FromAccount, at.Amount}).Must(details =>
                dataStore.IsDebitPossible(details.FromAccount, details.Amount)).When(at => at.Amount > 0).WithMessage("'From Account' has insufficient funds");
            RuleFor(at => at.Description).NotNull().MinimumLength(3);
        }
    }
}