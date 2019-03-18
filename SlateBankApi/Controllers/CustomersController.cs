using System;
using System.Collections.Generic;
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

        public CustomersController(IDataStore dataStore)
        {
            _dataStore = dataStore;
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
            return _dataStore.GetCustomer(id);
        }
        
        // POST api/values
        [HttpPost]
        public void Post([FromBody] Customer customer)
        {
            //_dataStore.AddCustomer(customer);

            
        }
        
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody] string value)
//        {
//        }
    }
}