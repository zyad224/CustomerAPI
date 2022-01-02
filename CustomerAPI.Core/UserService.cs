using CustomerAPI.Core.Interfaces;
using CustomerAPI.Dal.Interfaces;
using CustomerAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.Core
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;

        public UserService(IUserDal userDal)
        {
            _userDal = userDal;
        }
        public async Task<UserInfoReturnModel> AddUser(UserInfoModel userInfoModel)
        {
            return await _userDal.AddUser(userInfoModel);
        }

        public async Task<List<UserInfoReturnModel>> UserInfo(UserInfoModel userInfoModel)
        {
            return await _userDal.UserInfo(userInfoModel);
        }
    }
}
