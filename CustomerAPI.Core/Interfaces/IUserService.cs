using CustomerAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.Core.Interfaces
{
    public interface IUserService
    {
        public Task<UserInfoReturnModel> AddUser(UserInfoModel userInfoModel);
        public Task<List<UserInfoReturnModel>> UserInfo(UserInfoModel userInfoModel);
    }
}
