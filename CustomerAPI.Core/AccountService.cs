using CustomerAPI.Core.Interfaces;
using CustomerAPI.Dal;
using CustomerAPI.Dal.Interfaces;
using CustomerAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.Core
{
    public class AccountService : IAccountService
    {
        private readonly DbApiContext _dbContext;
        private readonly IAccountDal _accountDal;

        public AccountService(DbApiContext dbApiContext, IAccountDal accountDal)
        {
            _dbContext = dbApiContext;
            _accountDal = accountDal;
        }
        public async Task<bool> OpenNewAccountExistingUser(UserInfoModel userInfoModel)
        {
            return await _accountDal.OpenNewAccountExistingUser(userInfoModel);
        }

        public async Task<List<UserInfoReturnModel>> UserInfo(UserInfoModel userInfoModel)
        {
            return await _accountDal.UserInfo(userInfoModel);
        }
    }
}
