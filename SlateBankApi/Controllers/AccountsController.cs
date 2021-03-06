using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SlateBank.Core;
using SlateBank.Core.Entities;
using SlateBank.Core.Exceptions;

namespace SlateBankApi.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IDataStore _dataStore;

        public AccountsController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Account>> Get()
        {
            return _dataStore.GetAccounts();
        }
        
        // GET api/accounts/{id}
        [HttpGet("{id}")]
        public ActionResult<Account> Get(string id)
        {
            var account = _dataStore.GetAccount(id);

            return (ActionResult<Account>) account ?? NotFound();
        }
        
        [HttpPut("transaction")]
        public ActionResult<AccountTransaction> Transaction([FromBody] AccountTransaction accountTransaction)
        {
            try
            {
                if (accountTransaction.TransactionType == TransactionType.Debit)
                {
                    return _dataStore.Debit(accountTransaction);
                }

                return _dataStore.Credit(accountTransaction);
            }
            catch (InsufficientFundsException)
            {
                return BadRequest();
            }
        }
        
        [HttpPut("transfer")]
        public ActionResult<AccountTransfer> Transfer([FromBody] AccountTransfer accountTransfer)
        {
            try
            {
                return _dataStore.Transfer(accountTransfer);
            }
            catch (InsufficientFundsException)
            {
                return BadRequest();
            }
        }
    }
}