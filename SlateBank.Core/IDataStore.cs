using System.Collections.Generic;
using SlateBank.Core.Entities;

namespace SlateBank.Core
{
    public interface IDataStore
    {
        short IdentifierLength { get; }
        
        string GenerateCustomerID();

        bool AccountNumberExists(string accountNumber);

        string AddCustomer(Customer customer);

        Customer GetCustomer(string customerID);

        void DeleteCustomer(string customerID);

        void UpdateCustomer(Customer customer);

        List<Customer> GetCustomers();

        void CreateAccount(Account account);

        void DeleteAccount(string customerID);

        List<Account> GetAccounts();

        Account GetAccount(string accountNumber);

        void Deposit(AccountTransaction transaction);

        void Withdraw(AccountTransaction transaction);

        void Transfer(AccountTransfer transfer);
    }
}