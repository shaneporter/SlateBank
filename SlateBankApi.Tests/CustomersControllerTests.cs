using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SlateBank.Core;
using SlateBank.Core.Entities;
using SlateBankApi.Controllers;
using Xunit;

namespace SlateBankApi.IntegrationTests
{
    public class CustomerControllerTests
    {
        private readonly CustomersController _controller;
        private readonly Mock<IDataStore> _mock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IDataStore _dataStore;
        private readonly IMediator _mediator;

        public CustomerControllerTests()
        {
            _mock = new Mock<IDataStore>();
            _mediatorMock = new Mock<IMediator>();

            _mock.Setup(m => m.GetCustomers()).Returns(new List<Customer>
            {
                new Customer(),
                new Customer()
            });

            _mock.Setup(m => m.GetCustomer("100")).Returns(new Customer
            {
                Name = "Customer1"
            });

//            _mediatorMock
//                .Setup(m => m.Publish(It.IsAny<SomeEvent>()))
//                .ReturnsAsync(new Task());

            // setup mock:
            _dataStore = _mock.Object;
            _mediator = _mediatorMock.Object;
            _controller = new CustomersController(_dataStore, _mediator);
        }

        [Fact]
        public void Test_Get_Customers_Returns_Expected_Count()
        {
           var result = _controller.Get();
           Assert.True(result.Value.Count() == 2);
        }
        
        [Fact]
        public void Test_Get_Customer_Returns_Valid_Customer()
        {
            var result = _controller.Get("100");
            Assert.Equal("Customer1", result.Value.Name);
        }

        [Fact]
        public void Test_Get_Customer_With_Unknown_ID_Returns_Not_Found()
        {
            var result = _controller.Get("600");
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public void Test_Delete_Customer_With_Unknown_ID_Returns_Not_Found()
        {
            var result = _controller.Delete("600");
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Test_Delete_Customer_With_Valid_ID_Calls_Delete()
        {
            _controller.Delete("100");
            _mock.Verify(m => m.DeleteCustomer("100"), Times.Once);        
        }
        
        [Fact]
        public void Test_Update_Customer_With_Unknown_ID_Returns_Not_Found()
        {
            var result = _controller.Update(new Customer() { ID = "600" });
            Assert.IsType<NotFoundResult>(result.Result);
        }    
        
        [Fact]
        public void Test_Update_Customer_With_Valid_ID_Calls_Update()
        {
            var customerToUpdate = new Customer() {ID = "100"};
            _controller.Update(customerToUpdate);

            _mock.Verify(m => m.UpdateCustomer(customerToUpdate), Times.Once);
        }  

    }
}