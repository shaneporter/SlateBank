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
            RuleFor(at => at.Amount).GreaterThanOrEqualTo(0m);
            RuleFor(at => at.FromAccount).Must(dataStore.AccountNumberExists);
            RuleFor(at => at.ToAccount).Must(dataStore.AccountNumberExists);
            RuleFor(at => at.Description).NotNull().MinimumLength(3);
        }
    }
}