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

    [Route("User/")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

       
        [HttpPost]
        [Route("AddUser")]
        public async Task<ActionResult<string>>AddUser([FromBody]UserInfoModel userInfoModel)
        {
            if (string.IsNullOrEmpty(userInfoModel.FirstName) || string.IsNullOrEmpty(userInfoModel.SureName))
                return BadRequest("Null FirstName or Null SureName");

            var userInfoReturnModel = await _userService.AddUser(userInfoModel);
            var userInfoReturnModelJson = JsonConvert.SerializeObject(userInfoReturnModel);
            return Ok(userInfoReturnModelJson);
        }


        [HttpGet]
        [Route("UserInfo")]
        public async Task<ActionResult<string>> UserInfo([FromQuery] UserInfoModel userInfoModel)
        {
            if (string.IsNullOrEmpty(userInfoModel.CustomerId))
                return BadRequest("Null CustomerId");

            var userInfoReturnModel = await _userService.UserInfo(userInfoModel);
            var userInfoReturnModelJson = JsonConvert.SerializeObject(userInfoReturnModel);

            if (userInfoReturnModel == null)
                return BadRequest("No Such Customer Exists");

            return Ok(userInfoReturnModelJson);
        }

    }
}
