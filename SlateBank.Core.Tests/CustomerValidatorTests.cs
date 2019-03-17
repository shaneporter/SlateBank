using System;
using FluentValidation.TestHelper;
using SlateBank.Core.Entities;
using Xunit;

namespace SlateBank.Core.Tests
{
    public class CustomerValidatorTests
    {
        private CustomerValidator Validator { get; }

        public CustomerValidatorTests()
        {
            Validator = new CustomerValidator();
        }

        [Fact]
        public void Should_Pass_For_Valid_Name_Length()
        {
            var c = new Customer {Name = "Mrs Jane Doe"};
            Validator.ShouldNotHaveValidationErrorFor(customer => customer.Name, c);
            c.Name = "Shane";
            Validator.ShouldNotHaveValidationErrorFor(customer => customer.Name, c);
        }
        
        [Fact]
        public void Should_Fail_For_Invalid_Name_Length()
        {
            var c = new Customer {Name = "Andy"};
            Validator.ShouldHaveValidationErrorFor(customer => customer.Name, c);
            c.Name = null;
            Validator.ShouldHaveValidationErrorFor(customer => customer.Name, c);
        }

        [Fact]
        public void Should_Pass_For_Valid_Date_Of_Birth()
        {
            var c = new Customer {DateOfBirth = DateTime.Now.AddYears(-21)};
            Validator.ShouldNotHaveValidationErrorFor(customer => customer.DateOfBirth, c);
            
            c.DateOfBirth = DateTime.Now.AddYears(-18);
            Validator.ShouldNotHaveValidationErrorFor(customer => customer.DateOfBirth, c);
            
            c.DateOfBirth = DateTime.Now.AddYears(-99);
            Validator.ShouldNotHaveValidationErrorFor(customer => customer.DateOfBirth, c);
        }

        [Fact]
        public void Should_Fail_For_Invalid_Date_Of_Birth()
        {
            var c = new Customer {DateOfBirth = DateTime.Now.AddYears(-10)};
            Validator.ShouldHaveValidationErrorFor(customer => customer.DateOfBirth, c);
            
            c.DateOfBirth = DateTime.Now;
            Validator.ShouldHaveValidationErrorFor(customer => customer.DateOfBirth, c);
        }
        
        [Fact]
        public void Should_Pass_For_Valid_Address_Length()
        {
            var c = new Customer {Address = "100 Cherry Tree Lane, London"};
            Validator.ShouldNotHaveValidationErrorFor(customer => customer.Address, c);
            c.Address = "16 Oak Road, Belfast";
            Validator.ShouldNotHaveValidationErrorFor(customer => customer.Address, c);
        }
        
        [Fact]
        public void Should_Fail_For_Invalid_Address_Length()
        {
            var c = new Customer {Address = "Flat 2"};
            Validator.ShouldHaveValidationErrorFor(customer => customer.Address, c);
            c.Address = null;
            Validator.ShouldHaveValidationErrorFor(customer => customer.Address, c);
        }        
        /*
         *
            RuleFor(c => c.DateOfBirth)
                .InclusiveBetween(currentDate.AddYears(-100).AddDays(-1), currentDate.AddYears(-18));
            RuleFor(c => c.Address).MinimumLength(10);
            RuleFor(c => c.AccountNumber).Matches("^[0-9]{8}$");
         *
         * 
         */
    }
}