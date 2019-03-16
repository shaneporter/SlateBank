using System.Collections.Generic;
using SlateBank.Core.Entities;

namespace SlateBank.Core
{
    public interface IBankManager
    {
        /// <summary>
        /// Register new customer
        /// </summary>
        /// <param name="customer">Customer details</param>
        /// <returns>Customer ID</returns>
        string RegisterCustomer(Customer customer);
        
        /// <summary>
        /// Deregister customer
        /// </summary>
        /// <param name="customerID">Customer Identifier</param>
        /// <returns>True indicates customer was successfully removed</returns>
        bool DeregisterCustomer(string customerID);
        
        /// <summary>
        /// Updates a customer's details
        /// </summary>
        /// <param name="customer">Customer details</param>
        /// <returns>True indicates customer was successfully updated</returns>
        bool UpdateCustomer(Customer customer);

        /// <summary>
        /// Opens an account for a customer 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string OpenAccount(Account account);
        
        /// <summary>
        /// Closes an account for a customer
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        bool CloseAccount(string accountNumber);
        
        /// <summary>
        /// Debit or credit an account
        /// </summary>
        /// <param name="transactionDetails">The details of the transaction</param>
        /// <returns>True if the transaction was successful</returns>
        bool MakeTransaction(AccountTransaction transactionDetails);
        
        /// <summary>
        /// Transfer an amount between two bank accounts
        /// </summary>
        /// <param name="transferDetails">The details of the transfer</param>
        /// <returns>True if the transfer was successful</returns>
        bool Transfer(AccountTransfer transferDetails);

        /// <summary>
        /// Get a list of transactions for the given account
        /// </summary>
        /// <param name="accountNumber">Account identifier</param>
        /// <returns></returns>
        IEnumerable<AccountTransaction> GetAccountTransactions(string accountNumber);
    }
}