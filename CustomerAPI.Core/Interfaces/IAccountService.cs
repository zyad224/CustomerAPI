using CustomerAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.Core.Interfaces
{
    public interface IAccountService
    {
        public Task<bool> OpenNewAccountExistingUser(UserInfoModel userInfoModel);
        public Task<List<UserInfoReturnModel>> UserInfo(UserInfoModel userInfoModel);
    }
}
