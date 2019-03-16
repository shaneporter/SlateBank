using System.Collections.Generic;

namespace SlateBank.Core.Entities
{
    public class Account
    {
        public string CustomerID { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal OverdraftLimit { get; set; }
        public bool IsActive { get; set; }
        
        public IEnumerable<AccountTransaction> Transactions { get; set; }
    }
}