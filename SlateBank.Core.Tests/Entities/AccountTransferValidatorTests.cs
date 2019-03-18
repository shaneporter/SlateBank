using FluentValidation.TestHelper;
using Moq;
using SlateBank.Core.Entities;
using Xunit;

namespace SlateBank.Core.Tests.Entities
{
    public class AccountTransferValidatorTests
    {
        private AccountTransferValidator Validator { get; }

        public AccountTransferValidatorTests()
        {
            Validator = new AccountTransferValidator(new Mock<IDataStore>().Object);
        }

        private AccountTransferValidator GetAccountNumberExistsValidator(string accountNumber, bool shouldReturn)
        {
            // do some setup for constructor DI:
            var dataStoreMock = new Mock<IDataStore>();
            dataStoreMock.Setup(ds => ds.AccountNumberExists(accountNumber)).Returns(shouldReturn);
            return new AccountTransferValidator(dataStoreMock.Object);
        }
        
        [Fact]
        public void Should_Pass_For_Positive_Or_Zero_Amount()
        {
            var at = new AccountTransfer {Amount = 100m};
            Validator.ShouldNotHaveValidationErrorFor(a => a.Amount, at);
            at.Amount = 0.0m;
            Validator.ShouldNotHaveValidationErrorFor(a => a.Amount, at);
        }

        [Fact]
        public void Should_Fail_For_Negative_Amount()
        {
            var at = new AccountTransfer {Amount = -0.01m};
            Validator.ShouldHaveValidationErrorFor(a => a.Amount, at);
        }
        
        [Fact]
        public void Should_Pass_For_Valid_Description_Length()
        {
            var at = new AccountTransfer {Description = "Birthday Present"};
            Validator.ShouldNotHaveValidationErrorFor(a => a.Description, at);
            at.Description = "ABC";
            Validator.ShouldNotHaveValidationErrorFor(a => a.Description, at);
        }

        [Fact]
        public void Should_Fail_For_Invalid_Description_Length()
        {
            var at = new AccountTransfer {Description = "A"};
            Validator.ShouldHaveValidationErrorFor(a => a.Description, at);
            at.Description = null;
            Validator.ShouldHaveValidationErrorFor(a => a.Description, at);
        }
        
        [Fact]
        public void Should_Pass_For_Found_FromAccountNumber()
        {
            const string accountNumber = "100";
            var atv = GetAccountNumberExistsValidator(accountNumber, true);
            var accountTransfer = new AccountTransfer { FromAccount = accountNumber};
            atv.ShouldNotHaveValidationErrorFor(a => a.FromAccount, accountTransfer);
        }

        [Fact]
        public void Should_Fail_For_NotFound_FromAccountNumber()
        {
            const string accountNumber = "100";
            var atv = GetAccountNumberExistsValidator(accountNumber, false);
            var accountTransfer = new AccountTransfer { FromAccount = accountNumber};
            atv.ShouldHaveValidationErrorFor(a => a.FromAccount, accountTransfer);
        }
        
        [Fact]
        public void Should_Pass_For_Found_ToAccountNumber()
        {
            const string accountNumber = "100";
            var atv = GetAccountNumberExistsValidator(accountNumber, true);
            var accountTransfer = new AccountTransfer { ToAccount = accountNumber};
            atv.ShouldNotHaveValidationErrorFor(a => a.ToAccount, accountTransfer);
        }

        [Fact]
        public void Should_Fail_For_NotFound_ToAccountNumber()
        {
            const string accountNumber = "100";
            var atv = GetAccountNumberExistsValidator(accountNumber, false);
            var accountTransfer = new AccountTransfer { ToAccount = accountNumber};
            atv.ShouldHaveValidationErrorFor(a => a.ToAccount, accountTransfer);
        }
    }
}