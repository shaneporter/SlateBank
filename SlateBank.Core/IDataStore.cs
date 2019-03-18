using System.Collections.Generic;
using SlateBank.Core.Entities;

namespace SlateBank.Core
{
    public interface IDataStore
    {
        short IdentifierLength { get; }
        
        //string GenerateCustomerID();

        bool AccountNumberExists(string accountNumber);

        Customer AddCustomer(Customer customer);

        Customer GetCustomer(string customerID);

        Customer DeleteCustomer(string customerID);

        Customer UpdateCustomer(Customer customer);

        List<Customer> GetCustomers();
       
        List<Account> GetAccounts();

        Account GetAccount(string accountNumber);

        AccountTransaction Deposit(AccountTransaction transaction);

        AccountTransaction Withdraw(AccountTransaction transaction);

        AccountTransfer Transfer(AccountTransfer transfer);
    }
}