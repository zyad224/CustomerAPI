using CustomerAPI.Core.Interfaces;
using CustomerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Controllers
{
    [Route("Transaction/")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route("Deposit")]
        public async Task<ActionResult<string>> Deposit([FromBody] TransactionModel transactionModel)
        {
            if (string.IsNullOrEmpty(transactionModel.CustomerId) || string.IsNullOrEmpty(transactionModel.AccountId))
                return BadRequest("Null CustomerId or Null Account");
            if (transactionModel.Amount <= 0)
                return BadRequest("Enter Amount > 0");

            var transactionReturnModel = await _transactionService.DepositTransaction(transactionModel);
            var transactionReturnModelJson = JsonConvert.SerializeObject(transactionReturnModel);
            return Ok(transactionReturnModelJson);
        }

        [HttpPost]
        [Route("Withdraw")]
        public async Task<ActionResult<string>> Withdraw([FromBody] TransactionModel transactionModel)
        {
            if (string.IsNullOrEmpty(transactionModel.CustomerId) || string.IsNullOrEmpty(transactionModel.AccountId))
                return BadRequest("Null CustomerId or Null Account");
            if (transactionModel.Amount >= 0)
                return BadRequest("Enter Amount < 0");

            var transactionReturnModel = await _transactionService.WithDrawTransaction(transactionModel);
            var transactionReturnModelJson = JsonConvert.SerializeObject(transactionReturnModel);
            return Ok(transactionReturnModelJson);
        }

        [HttpGet]
        [Route("CheckBalance")]
        public async Task<ActionResult<string>> CheckBalance([FromQuery] TransactionModel transactionModel)
        {
            if (string.IsNullOrEmpty(transactionModel.CustomerId) || string.IsNullOrEmpty(transactionModel.AccountId))
                return BadRequest("Null CustomerId or Null Account");
            

            var transactionReturnModel = await _transactionService.CheckBalanceTransaction(transactionModel);
            var transactionReturnModelJson = JsonConvert.SerializeObject(transactionReturnModel);
            return Ok(transactionReturnModelJson);
        }
    }
}
