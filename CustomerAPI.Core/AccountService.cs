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
        private readonly IAccountDal _accountDal;

        public AccountService(IAccountDal accountDal)
        {
            _accountDal = accountDal;
        }
        public async Task<bool> OpenNewAccountExistingUser(UserInfoModel userInfoModel)
        {
            return await _accountDal.OpenNewAccountExistingUser(userInfoModel);
        }
   
    }
}
