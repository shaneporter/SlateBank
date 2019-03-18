using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlateBank.Core;
using SlateBank.Core.Entities;

namespace SlateBankApi.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        // TODO: inject this dependency
        DataStore _dataStore = new DataStore();
        
        // GET api/customers
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            return _dataStore.GetCustomers();
        }
    }
}