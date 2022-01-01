using CustomerAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.Dal.Interfaces
{
    public interface IAccountDal
    {
        public Task<bool> OpenNewAccountExistingUser(UserInfoModel userInfoModel);
        public Task<List<UserInfoReturnModel>> UserInfo(UserInfoModel userInfoModel);
    }
}
