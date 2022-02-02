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

    [Route("api/User/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;//
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

            if (userInfoReturnModel == null)
                return NotFound("No Such Customer Exists");

            var userInfoReturnModelJson = JsonConvert.SerializeObject(userInfoReturnModel);
            return Ok(userInfoReturnModelJson);
        }

    }
}
