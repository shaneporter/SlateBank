using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SlateBank.Core;
using SlateBank.Core.Entities;
using SlateBank.Core.Exceptions;
using SlateBankApi.Controllers;
using Xunit;

namespace SlateBankApi.IntegrationTests
{
    public class AccountsControllerTests
    {
        private readonly AccountsController _controller;
        private readonly Mock<IDataStore> _mock;
        private readonly IDataStore _dataStore;

        public AccountsControllerTests()
        {
            _mock = new Mock<IDataStore>();

            _mock.Setup(m => m.GetAccounts()).Returns(new List<Account>
            {
                new Account(),
                new Account(),
                new Account()
            });

            _mock.Setup(m => m.GetAccount("100")).Returns(new Account
            {
                AccountNumber = "100",
                CustomerID = "001"
            });

            // setup mock:
            _dataStore = _mock.Object;
            _controller = new AccountsController(_dataStore);
        }
        
        [Fact]
        public void Test_Get_Accounts_Returns_Expected_Count()
        {
            var result = _controller.Get();
            Assert.True(result.Value.Count() == 3);
        }

        [Fact]
        public void Test_Get_Account_Returns_Valid_Customer()
        {
            var result = _controller.Get("100");
            Assert.Equal("100", result.Value.AccountNumber);
            Assert.Equal("001", result.Value.CustomerID);
        }

        [Fact]
        public void Test_That_Throwing_Withdraw_Returns_Bad_Request()
        {
            var at = new AccountTransaction() { Amount = 19.99m };
            
            _mock.Setup(m => m.Withdraw(at)).Throws(new InsufficientFundsException());
            var result = _controller.Withdraw(at);
            Assert.IsType<BadRequestResult>(result.Result);
        }
        
        [Fact]
        public void Test_That_Valid_Withdraw_Calls_Withdraw_Once()
        {
            var at = new AccountTransaction() { Amount = 19.99m };

            _mock.Setup(m => m.Withdraw(at)).Returns(at);
            var result = _controller.Withdraw(at);
            _mock.Verify(m => m.Withdraw(at), Times.Once);
        }
        
        [Fact]
        public void Test_That_Valid_Deposit_Calls_Deposit_Once()
        {
            var at = new AccountTransaction() { Amount = 19.99m };

            _mock.Setup(m => m.Deposit(at)).Returns(at);
            var result = _controller.Deposit(at);
            _mock.Verify(m => m.Deposit(at), Times.Once);
        }
        
        [Fact]
        public void Test_That_Throwing_Transfer_Returns_Bad_Request()
        {
            var at = new AccountTransfer() { Amount = 19.99m };
            
            _mock.Setup(m => m.Transfer(at)).Throws(new InsufficientFundsException());
            var result = _controller.Transfer(at);
            Assert.IsType<BadRequestResult>(result.Result);
        }
        
        [Fact]
        public void Test_That_Successful_Transfer_Calls_Transfer_Once()
        {
            var at = new AccountTransfer() { Amount = 19.99m };

            _mock.Setup(m => m.Transfer(at)).Returns(at);
            var result = _controller.Transfer(at);
            _mock.Verify(m => m.Transfer(at), Times.Once);
        }
    }
}