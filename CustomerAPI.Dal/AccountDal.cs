using CustomerAPI.Dal.Interfaces;
using CustomerAPI.Entities;
using CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.Dal
{
    public class AccountDal : IAccountDal
    {
        private readonly DbApiContext _dbContext;
        private readonly ITransactionDal _transactionDal;

        public AccountDal(DbApiContext dbApiContext, ITransactionDal transactionDal)
        {
            _dbContext = dbApiContext;
            _transactionDal = transactionDal;
        }
        public async Task<bool> OpenNewAccountExistingUser(UserInfoModel userInfoModel)
        {
            var userDb = await GetUser(userInfoModel.CustomerId);

            if (userDb == null)
                return false;

            TransactionModel transactionModel = new TransactionModel() { CustomerId = userDb.UserId, Amount = userInfoModel.InitialCredit };
            return await _transactionDal.OpenNewAccountExistingUserTransaction(transactionModel);
        }

 
        private async Task<User> GetUser(string UserId)
        {
            return await _dbContext
                         .Users
                         .Where(user => user.UserId == UserId).FirstOrDefaultAsync();
        }
    }
}
