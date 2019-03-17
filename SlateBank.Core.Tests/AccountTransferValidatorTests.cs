using FluentValidation.TestHelper;
using SlateBank.Core.Entities;
using Xunit;

namespace SlateBank.Core.Tests
{
    public class AccountTransferValidatorTests
    {
        private AccountTransferValidator Validator { get; }

        public AccountTransferValidatorTests()
        {
            Validator = new AccountTransferValidator();
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
        
        // TODO FromAccount tests
        // TODO ToAccount tests
    }
}