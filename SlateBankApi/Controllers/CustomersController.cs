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
        private IDataStore _dataStore;

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
    }
}