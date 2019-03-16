namespace SlateBank.Core.Entities
{
    public class AccountTransfer
    {
        public decimal Amount { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public string Description { get; set; }
    }
}