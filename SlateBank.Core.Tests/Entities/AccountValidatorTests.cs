using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation.TestHelper;
using Moq;
using Xunit;
using SlateBank.Core.Entities;
using SlateBank.Core.Tests.Utils;

namespace SlateBank.Core.Tests.Entities
{
    public class AccountValidatorTests
    {
        private AccountValidator Validator { get; }
        private short IdentifierLength => 8;

        public AccountValidatorTests()
        {
            var dataStoreMock = new Mock<IDataStore>();
            dataStoreMock.SetupGet(dsm => dsm.IdentifierLength).Returns(8);
            
            Validator = new AccountValidator(dataStoreMock.Object);
        }

        private void TestLength(int expectedLength, Expression<Func<Account, string>> exp)
        {
            var account = new Account();
            
            for (var length = 0; length < expectedLength; length++)
            {                
                account.SetPropertyValue(exp, new string('0', length));
                Validator.ShouldHaveValidationErrorFor(exp, account);
            }
            
            account.SetPropertyValue(exp, new string('0', expectedLength));
            Validator.ShouldNotHaveValidationErrorFor(exp, account);
        }
        
        [Fact]
        public void Should_Pass_For_Customer_Length()
        {
            TestLength(IdentifierLength, a => a.CustomerID);
        }

        [Fact]
        public void Should_Pass_For_AccountNumber_Length()
        {
            TestLength(IdentifierLength, a => a.AccountNumber);
        }

        [Fact]
        public void Should_Pass_For_OverdraftLimit()
        {
            var account = new Account();
            var expectedRange = new Tuple<short, short>(0, 1000);

            for (var loop = expectedRange.Item1; loop <= expectedRange.Item2; loop++)
            {
                account.OverdraftLimit = loop;
                Validator.ShouldNotHaveValidationErrorFor(a => a.OverdraftLimit, account);
            }
        }
    }
}