using System.Collections.Generic;
using SlateBank.Core.Entities;

namespace SlateBank.Core
{
    public interface IDataStore
    {
        short IdentifierLength { get; }
        
        //string GenerateCustomerID();

        bool AccountNumberExists(string accountNumber);

        bool IsAccountActive(string accountNumber);

        bool IsDebitPossible(string accountNumber, decimal amount);

        Customer AddCustomer(Customer customer);

        Customer GetCustomer(string customerID);

        Customer DeleteCustomer(string customerID);

        Customer UpdateCustomer(Customer customer);

        List<Customer> GetCustomers();
       
        List<Account> GetAccounts();

        Account GetAccount(string accountNumber);

        AccountTransaction Credit(AccountTransaction transaction);

        AccountTransaction Debit(AccountTransaction transaction);

        AccountTransfer Transfer(AccountTransfer transfer);
    }
}