using System;
using FluentValidation;

namespace SlateBank.Core.Entities
{
    public class Customer
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string AccountNumber { get; set; }
        public bool IsActive { get; set; }
    }

    public class CustomerValidator : AbstractValidator<Customer>
    {
        private int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {
            //Excel documentation says "COMPLETE calendar years in between dates"
            int years = endDate.Year - startDate.Year;

            if (startDate.Month == endDate.Month &&// if the start month and the end month are the same
                endDate.Day < startDate.Day// AND the end day is less than the start day
                || endDate.Month < startDate.Month)// OR if the end month is less than the start month
            {
                years--;
            }

            return years;
        }
        public CustomerValidator()
        {            
            RuleFor(c => c.Name).NotNull().MinimumLength(5);
            RuleFor(c => c.DateOfBirth).Must(dob => GetDifferenceInYears(dob, DateTime.Now) >= 18);
            RuleFor(c => c.Address).NotNull().MinimumLength(10);
            RuleFor(c => c.AccountNumber).NotNull().Matches("^[0-9]{8}$");
        }
    }
}
