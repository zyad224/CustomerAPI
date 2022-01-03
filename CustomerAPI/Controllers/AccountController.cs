using CustomerAPI.Core.Interfaces;
using CustomerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CustomerAPI.Controllers
{
    [Route("Account/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("OpenAccount")]
        public async Task<ActionResult> OpenNewAccountExistingUser([FromBody] UserInfoModel userInfoModel)
        {

            if (string.IsNullOrEmpty(userInfoModel.CustomerId))
                return BadRequest("Bad CustomerId");

            var openAccount = await _accountService.OpenNewAccountExistingUser(userInfoModel);

            if (openAccount)
                return Ok(openAccount);

            return BadRequest(openAccount);
          
        }
    }
}
