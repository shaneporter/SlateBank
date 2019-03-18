using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using SlateBank.Core.Entities;

namespace SlateBank.Core
{
    public class DataStore : IDataStore
    {
        private List<Customer> _customers = new List<Customer>();
        private List<Account> _accounts = new List<Account>();
        
        private static short CustomerIdentifierLength = 8;
        private static short AccountNumberLength = 8;
            
        public DataStore()
        {
            // TODO: generate some dummy data:
        }
        
        public string GenerateCustomerID()
        {
            // sequential:
            var countString = (_customers.Count + 1).ToString();

            return countString.PadLeft(CustomerIdentifierLength - countString.Length);
        }

        public string GenerateAccountNumber()
        {
            // sequential, but prefixed with a 1:
            var countString = (_accounts.Count + 1).ToString();

            return '1' + countString.PadLeft(AccountNumberLength - countString.Length - 1);
        }

        public bool AccountNumberExists(string accountNumber)
        {
            return (from a in _accounts
                where a.AccountNumber == accountNumber
                select a).Any();
        }

        public void AddCustomer(Customer customer)
        {
            _customers.Add(customer);
        }

        public Customer GetCustomer(string customerID)
        {
            return (from c in _customers
                where c.ID == customerID
                select c).First();
        }

        public void DeleteCustomer(string customerID)
        {
            var customerToDelete = GetCustomer(customerID);

            customerToDelete.IsActive = false;
            
            DeleteAccount(customerID);
        }

        public void UpdateCustomer(Customer customer)
        {
            var customerToUpdate = (from c in _customers
                where c.ID == customer.ID
                select c).First();
        }

        public void CreateAccount(Account account)
        {
            _accounts.Add(account);
        }

        public void DeleteAccount(string customerID)
        {
            // update the account for this customer:
            var accountToDelete = (from a in _accounts
                where a.CustomerID == customerID
                select a).First();

            accountToDelete.IsActive = false;        
        }

        private Account GetAccount(string accountNumber)
        {
            return (from a in _accounts
                where a.AccountNumber == accountNumber
                select a).First();
        }
        
        public void Deposit(AccountTransaction transaction)
        {
            var account = GetAccount(transaction.AccountNumber);
            account.Transactions.Add(transaction);
            account.Balance += transaction.Amount;
        }

        public void Withdraw(AccountTransaction transaction)
        {
            var account = GetAccount(transaction.AccountNumber);
            account.Transactions.Add(transaction);
            account.Balance -= transaction.Amount;        
        }

        public void Transfer(AccountTransfer transfer)
        {
            // get customer details
            var fromAccount = GetAccount(transfer.FromAccount);
            var toAccount = GetAccount(transfer.ToAccount);

            var fromCustomer = GetCustomer(fromAccount.CustomerID);
            var toCustomer = GetCustomer(toAccount.CustomerID);
                
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

            Withdraw(fromTransaction);
            Deposit(toTransaction);
        }
    }
}