using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading.Tasks;
using MediatR;
using SlateBank.Core.Entities;
using SlateBank.Core.Events;
using SlateBank.Core.Exceptions;

namespace SlateBank.Core
{
    public class DataStore : IDataStore
    {
        private readonly List<Customer> _customers = new List<Customer>();
        private readonly List<Account> _accounts = new List<Account>();

        public DataStore()
        {
            #region Generate dummy data

            for (var loop = 0; loop < 5; loop++)
            {
                var accountNumber = GenerateAccountNumber();
                var customerID = GenerateCustomerID();

                _customers.Add(new Customer
                {
                    ID = customerID,
                    Name = $"User {loop + 1}",
                    DateOfBirth = new DateTime(1990 - (loop * 2), (loop + 1) * 2, (loop + 4) * 2),
                    Address = $"Flat {loop + 1}, 120 Beach View, Southend",
                    AccountNumber = accountNumber,
                    IsActive = loop != 3
                });

                _accounts.Add(new Account()
                {
                    CustomerID = customerID,
                    AccountNumber = accountNumber,
                    Balance = loop + 2000 + new Random().Next(0, 2500),
                    IsActive = loop != 3,
                    OverdraftLimit = loop % 2 == 0 ? 500 : 0,
                    Transactions = new List<AccountTransaction>()
                });
            }

            #endregion
        }

        public short IdentifierLength => 8;

        public string GenerateCustomerID()
        {
            // sequential:
            var countString = (_customers.Count + 1).ToString();

            return countString.PadLeft(IdentifierLength - countString.Length + 1, '0');
        }

        public string GenerateAccountNumber()
        {
            // sequential, but prefixed with a 1:
            var countString = (_accounts.Count + 1).ToString();

            return '1' + countString.PadLeft(IdentifierLength - countString.Length, '0');
        }

        public bool AccountNumberExists(string accountNumber)
        {
            return (from a in _accounts
                where a.AccountNumber == accountNumber
                select a).Any();
        }

        public bool IsAccountActive(string accountNumber)
        {
            var account = GetAccount(accountNumber);

            return !(account == null || !account.IsActive);
        }

        public bool IsDebitPossible(string accountNumber, decimal amount)
        {
            if (!IsAccountActive(accountNumber))
                return false;

            // refactor this - code above already calls GetAccount:
            var account = GetAccount(accountNumber);
            
            return !(account.Balance - amount < -account.OverdraftLimit);
        }

        public Customer AddCustomer(Customer customer)
        {
            // generate the ID:
            customer.ID = GenerateCustomerID();
            
            // 0 the date of birth:
            customer.DateOfBirth = customer.DateOfBirth.Date;
            
            // create an account:    
            var accountNumber = CreateAccount(customer.ID);

            customer.AccountNumber = accountNumber;
            customer.IsActive = true;
                
            // add the customer:
            _customers.Add(customer);
            
            return customer;
        }

        public Customer GetCustomer(string customerID)
        {
            return (from c in _customers
                where c.ID == customerID
                select c).FirstOrDefault();
        }

        public List<Customer> GetCustomers()
        {            
            return _customers;
        }

        public Customer DeleteCustomer(string customerID)
        {
            var customerToDelete = GetCustomer(customerID);
            
            var accountToDelete = (from a in _accounts
                where a.CustomerID == customerID
                select a).FirstOrDefault();

            customerToDelete.IsActive = false;

            // shouldn't be null, over-defensive?
            if (accountToDelete != null)
            {
                DeleteAccount(accountToDelete.AccountNumber);
            }

            return customerToDelete;
        }

        public Customer UpdateCustomer(Customer customer)
        {
            var customerToUpdate = (from c in _customers
                where c.ID == customer.ID
                select c).FirstOrDefault();

            if (customerToUpdate == null)
            {
                throw new CustomerNotFoundException("Customer not found");
            }
            
            customerToUpdate.Name = customer.Name;
            customerToUpdate.Address = customer.Address;
            customerToUpdate.DateOfBirth = customer.DateOfBirth;

            return customerToUpdate;
        }

        private string CreateAccount(string customerID)
        {
            var newAccount = new Account
            {
                AccountNumber = GenerateAccountNumber(),
                CustomerID = customerID,
                IsActive = true,
                Transactions = new List<AccountTransaction>()
            };

            _accounts.Add(newAccount);
            return newAccount.AccountNumber;
        }

        private void DeleteAccount(string accountNumber)
        {
            // update the account for this customer:
            var accountToDelete = (from a in _accounts
                where a.AccountNumber == accountNumber
                select a).FirstOrDefault();

            if (accountToDelete != null)
            {
                accountToDelete.IsActive = false;        
            }
        }

        public Account GetAccount(string accountNumber)
        {
            return (from a in _accounts
                where a.AccountNumber == accountNumber
                select a).FirstOrDefault();
        }

        public List<Account> GetAccounts()
        {
            return _accounts;
        }
        
        public AccountTransaction Credit(AccountTransaction transaction)
        {
            var account = GetAccount(transaction.AccountNumber);

            if (account == null)
            {
                throw new AccountException("Cannot credit to inactive account");
            }
            
            transaction.TransactionType = TransactionType.Debit;

            if (account.Transactions == null)
            {
                account.Transactions = new List<AccountTransaction>();
            }
            account.Transactions.Add(transaction);
            account.Balance += transaction.Amount;

            return transaction;
        }

        public AccountTransaction Debit(AccountTransaction transaction)
        {
            var account = GetAccount(transaction.AccountNumber);
            
            if (account == null)
            {
                throw new AccountException("Cannot debit from inactive account");
            }
            
            transaction.TransactionType = TransactionType.Credit;
            
            if (account.Balance - transaction.Amount < -account.OverdraftLimit)
            {
                throw new InsufficientFundsException("Account has insufficient funds for debit");
            }
            
            if (account.Transactions == null)
            {
                account.Transactions = new List<AccountTransaction>();
            }
            account.Transactions.Add(transaction);
            account.Balance -= transaction.Amount;

            return transaction;
        }

        public AccountTransfer Transfer(AccountTransfer transfer)
        {
            // get customer details
            var fromAccount = GetAccount(transfer.FromAccount);
            var toAccount = GetAccount(transfer.ToAccount);

            if (!fromAccount.IsActive)
            {
                throw new AccountException("From account inactive");
            }

            if (!toAccount.IsActive)
            {
                throw new AccountException("To account inactive");
            }

            var fromCustomer = GetCustomer(fromAccount.CustomerID);
            var toCustomer = GetCustomer(toAccount.CustomerID);
            
            // does from account have enough funds for the transaction?
            if (fromAccount.Balance - transfer.Amount < -fromAccount.OverdraftLimit)
            {
                throw new InsufficientFundsException("From account has insufficient funds for transfer");
            }
            
            // create two transactions based on the transfer:
            var fromTransaction = new AccountTransaction
            {
                AccountNumber = transfer.FromAccount,
                Amount = transfer.Amount,
                Description = $"{transfer.Description}. Transfer to {toCustomer.Name}",
                TransactionType = TransactionType.Transfer
            };
            var toTransaction = new AccountTransaction
            {
                AccountNumber = transfer.ToAccount,
                Amount = transfer.Amount,
                Description = $"{transfer.Description}. Transfer from {fromCustomer.Name}",
                TransactionType = TransactionType.Transfer
            };

            Debit(fromTransaction);
            Credit(toTransaction);
            
            return transfer;
        }
    }
}