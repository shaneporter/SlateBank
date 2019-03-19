using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SlateBank.Core;
using SlateBank.Core.Entities;

namespace SlateBankApi.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IDataStore _dataStore;
        private readonly IMediator _mediator;

        public CustomersController(IDataStore dataStore, IMediator mediator)
        {
            _dataStore = dataStore;
            _mediator = mediator;
        }
        
        // GET api/customers
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            return _dataStore.GetCustomers();
        }
        
        // GET api/customers/{id}
        [HttpGet("{id}")]
        public ActionResult<Customer> Get(string id)
        {
            var customer = _dataStore.GetCustomer(id);

            if (customer != null)
            {
                return customer;
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<Customer> Post([FromBody] Customer customer)
        {
            return _dataStore.AddCustomer(customer);
        }

        [HttpDelete("{id}")]
        public ActionResult<Customer> Delete(string id)
        {
            var customer = _dataStore.GetCustomer(id);

            if (customer == null)
            {
                return NotFound();
            }
            
            return _dataStore.DeleteCustomer(id);
        }

        [HttpPut("{id}")]
        public ActionResult<Customer> Update([FromBody] Customer customer)
        {
            var customerToUpdate = _dataStore.GetCustomer(customer.ID);

            if (customerToUpdate == null)
            {
                return NotFound();
            }

            return _dataStore.UpdateCustomer(customer);
        }
    }
}