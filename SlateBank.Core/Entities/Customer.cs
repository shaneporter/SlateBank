using System;

namespace SlateBank.Core.Entities
{
    public class Customer
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string AccountNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
