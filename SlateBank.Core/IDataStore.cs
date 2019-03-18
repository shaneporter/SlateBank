using System.Collections.Generic;
using SlateBank.Core.Entities;

namespace SlateBank.Core
{
    public interface IDataStore
    {
        string GenerateCustomerID();

        bool AccountNumberExists(string accountNumber);

        void AddCustomer(Customer customer);

        Customer GetCustomer(string customerID);

        void DeleteCustomer(string customerID);

        void UpdateCustomer(Customer customer);

        List<Customer> GetCustomers();

        void CreateAccount(Account account);

        void DeleteAccount(string customerID);

        void Deposit(AccountTransaction transaction);

        void Withdraw(AccountTransaction transaction);

        void Transfer(AccountTransfer transfer);
    }
}