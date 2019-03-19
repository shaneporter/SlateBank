using System;
using SlateBank.Core.Entities;
using SlateBank.Core.Exceptions;
using Xunit;

namespace SlateBank.Core.Tests
{
    public class DataStoreTests
    {
        [Fact]
        public void Default_Data_Store_Should_Have_Expected_Dummy_Data()
        {
            var ds = new DataStore();
            var expectedCount = 5;

            Assert.Equal(expectedCount, ds.GetCustomers().Count);
            Assert.Equal(expectedCount, ds.GetAccounts().Count);
        }

        [Fact]
        public void Identifier_Length_Is_Expected_Value()
        {
            var ds = new DataStore();
            Assert.Equal(8, ds.IdentifierLength);
        }

        [Fact]
        public void Customer_ID_Is_Expected_Value()
        {
            // account for those first 5 accounts:
            Assert.Equal("10000006", new DataStore().GenerateAccountNumber());
        }

        [Fact]
        public void Expected_Account_Numbers_Exist_At_Creation()
        {
            var ds = new DataStore();

            for (var loop = 0; loop < 5; loop++)
            {
                Assert.True(ds.AccountNumberExists($"1000000{loop + 1}"));
            }
        }

        [Fact]
        public void Add_Customer_Updates_Customers_With_Expected_Value()
        {
            var ds = new DataStore();
            var newCustomer = new Customer
            {
                Address = "100 High Street, Portsmouth",
                DateOfBirth = new DateTime(1950, 1, 1),
                Name = "Martin Peters"
            };
            
            var customer = ds.AddCustomer(newCustomer);

            var addedCustomer = ds.GetCustomer(customer.ID);
            var addedBankAccount = ds.GetAccount(addedCustomer.AccountNumber);

            Assert.Equal(customer.ID, addedCustomer.ID);
            Assert.Equal(newCustomer.Address, addedCustomer.Address);
            Assert.Equal(newCustomer.DateOfBirth, addedCustomer.DateOfBirth);
            Assert.Equal(newCustomer.Name, addedCustomer.Name);

            Assert.Equal(0, addedBankAccount.Transactions.Count);
            Assert.True(addedBankAccount.IsActive);
        }

        [Fact]
        public void Delete_Customer_Updates_IsActive_Flag()
        {
            var ds = new DataStore();
            const string customerID = "00000001";

            var customerToDelete = ds.GetCustomer(customerID);
            Assert.True(customerToDelete.IsActive);
            
            ds.DeleteCustomer(customerID);
            var deletedCustomer = ds.GetCustomer(customerID);
            Assert.False(deletedCustomer.IsActive);
        }

        [Fact]
        public void Update_Customer_Throws_If_Customer_Not_Found()
        {
            var ds = new DataStore();
            var customer = new Customer()
            {
                ID = "00000009"
            };

            var ex = Record.Exception(() => { ds.UpdateCustomer(customer); });
            
            Assert.NotNull(ex);
            Assert.IsType<CustomerNotFoundException>(ex);
        }

        [Fact]
        public void Update_Customer_Updates_Expected_Fields()
        {
            var ds = new DataStore();

            var customer = ds.GetCustomer("00000001");
            
            customer.Name = "Adrian Andrews";
            customer.Address = "62 Park Avenue, Brighton";
            customer.DateOfBirth = new DateTime(1945, 8, 12);
            
            ds.UpdateCustomer(customer);
            
            var updatedCustomer = ds.GetCustomer("00000001");
            
            Assert.Equal(customer.Name, updatedCustomer.Name);
            Assert.Equal(customer.Address, updatedCustomer.Address);
            Assert.Equal(customer.DateOfBirth, updatedCustomer.DateOfBirth);
        }

        [Fact]
        public void Delete_User_Updates_Active_Flag()
        {
            var ds = new DataStore();
            const string customerNumber = "00000001";
            const string accountNumber = "10000001";
            
            Assert.True(ds.GetAccount(accountNumber).IsActive); 
            
            ds.DeleteCustomer(customerNumber);
            
            Assert.False(ds.GetCustomer(customerNumber).IsActive); 
            Assert.False(ds.GetAccount(accountNumber).IsActive); 
        }

        [Fact]
        public void Withdraw_Too_Much_Throws_And_Does_Not_Add_Transaction()
        {
            var ds = new DataStore();
            const string accountNumber = "10000001";
            var ex = Record.Exception(() => ds.Debit(new AccountTransaction
            {
                Amount = 5000m,
                AccountNumber = accountNumber
            }));
            
            Assert.NotNull(ex);
            Assert.IsType<InsufficientFundsException>(ex);
            Assert.Equal(0, ds.GetAccount(accountNumber).Transactions.Count);
        }
        
        [Fact]
        public void Withdraw_Allowable_Updates_Balance_And_Transactions()
        {
            var ds = new DataStore();
            const string accountNumber = "10000001";
            const decimal amountToWithdraw = 10m;

            var account = ds.GetAccount(accountNumber);
            var initialBalance = account.Balance;
            
            ds.Debit(new AccountTransaction
            {
                Amount = amountToWithdraw,
                AccountNumber = accountNumber
            });

            var updatedAccount = ds.GetAccount(accountNumber);
            
            Assert.True(updatedAccount.Balance + amountToWithdraw == initialBalance);
            Assert.True(updatedAccount.Transactions.Count == 1);
        }

        [Fact]
        public void Transfer_Too_Much_Throws_And_Does_Not_Add_Transactions()
        {
            var ds = new DataStore();
            const string fromAccount = "10000001";
            const string toAccount = "10000002";
            var ex = Record.Exception(() => ds.Transfer(new AccountTransfer()
            {
                Amount = 5000m,
                FromAccount = fromAccount,
                ToAccount = toAccount
            }));
            
            Assert.NotNull(ex);
            Assert.IsType<InsufficientFundsException>(ex);
            
            Assert.Equal(0, ds.GetAccount(fromAccount).Transactions.Count);
            Assert.Equal(0, ds.GetAccount(toAccount).Transactions.Count);

        }

        [Fact]
        public void Valid_Transfer_Updates_Balances_And_Transactions()
        {
            var ds = new DataStore();
            const string fromAccount = "10000001";
            const string toAccount = "10000002";
            const decimal transferAmount = 100m;
            
            var transfer = new AccountTransfer
            {
                Amount = transferAmount,
                FromAccount = fromAccount,
                ToAccount = "10000002"
            };

            var fromAccountBeforeBalance = ds.GetAccount(fromAccount).Balance;
            var toAccountBeforeBalance = ds.GetAccount(toAccount).Balance;
          
            ds.Transfer(transfer);

            var fromAccountAfterBalance = ds.GetAccount(fromAccount).Balance;
            var toAccountAfterBalance = ds.GetAccount(toAccount).Balance;

            Assert.True(fromAccountBeforeBalance - transferAmount == fromAccountAfterBalance);
            Assert.True( toAccountBeforeBalance + transferAmount == toAccountAfterBalance);
            
            Assert.Equal(1, ds.GetAccount(fromAccount).Transactions.Count);
            Assert.Equal(1, ds.GetAccount(toAccount).Transactions.Count);
        }
    }
}