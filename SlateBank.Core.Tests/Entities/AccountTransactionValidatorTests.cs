using System.Diagnostics;
using FluentValidation.TestHelper;
using SlateBank.Core.Entities;
using Xunit;
using Moq;

namespace SlateBank.Core.Tests.Entities
{
    public class AccountTransactionValidatorTests
    {
        private AccountTransactionValidator Validator { get; }

        public AccountTransactionValidatorTests()
        {
            // use dummy for default Validator constructor parameter:
            Validator = new AccountTransactionValidator(new Mock<IDataStore>().Object);
        }

        [Fact]
        public void Should_Pass_For_Positive_Or_Zero_Amount()
        {
            var at = new AccountTransaction {Amount = 100m};
            Validator.ShouldNotHaveValidationErrorFor(a => a.Amount, at);
            at.Amount = 0.0m;
            Validator.ShouldNotHaveValidationErrorFor(a => a.Amount, at);
        }

        [Fact]
        public void Should_Fail_For_Negative_Amount()
        {
            var at = new AccountTransaction {Amount = -0.01m};
            Validator.ShouldHaveValidationErrorFor(a => a.Amount, at);
        }

        [Fact]
        public void Should_Pass_For_Valid_Description_Length()
        {
            var at = new AccountTransaction {Description = "Salary Payment"};
            Validator.ShouldNotHaveValidationErrorFor(a => a.Description, at);
            at.Description = "ABC";
            Validator.ShouldNotHaveValidationErrorFor(a => a.Description, at);
        }

        [Fact]
        public void Should_Fail_For_Invalid_Description_Length()
        {
            var at = new AccountTransaction {Description = "A"};
            Validator.ShouldHaveValidationErrorFor(a => a.Description, at);
            at.Description = null;
            Validator.ShouldHaveValidationErrorFor(a => a.Description, at);
        }

        [Fact]
        public void Should_Pass_For_Found_AccountNumber()
        {
            // do some setup for constructor DI:
            var dataStoreMock = new Mock<IDataStore>();
            const string accountNumber = "100";
            dataStoreMock.Setup(ds => ds.AccountNumberExists(accountNumber)).Returns(true);
            
            var atv = new AccountTransactionValidator(dataStoreMock.Object);
            var accountTransaction = new AccountTransaction {AccountNumber = accountNumber};
            atv.ShouldNotHaveValidationErrorFor(a => a.AccountNumber, accountTransaction);
        }

        [Fact]
        public void Should_Fail_For_NotFound_AccountNumber()
        {
            // do some setup for constructor DI:
            var dataStoreMock = new Mock<IDataStore>();
            const string accountNumber = "100";
            dataStoreMock.Setup(ds => ds.AccountNumberExists(accountNumber)).Returns(false);

            var atv = new AccountTransactionValidator(dataStoreMock.Object);
            var accountTransaction = new AccountTransaction {AccountNumber = accountNumber};
            atv.ShouldHaveValidationErrorFor(a => a.AccountNumber, accountTransaction);
        }
    }
}