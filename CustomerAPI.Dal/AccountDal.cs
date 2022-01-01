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

            TransactionModel transactionModel = new TransactionModel() { UserId = userDb.UserId, Amount = userInfoModel.InitialCredit };
            return await _transactionDal.OpenNewAccountExistingUserTransaction(transactionModel);
        }

        public async Task<List<UserInfoReturnModel>> UserInfo(UserInfoModel userInfoModel)
        {

            return await _dbContext.Accounts.Where(account => account.User.UserId == userInfoModel.CustomerId)
                   .Include(account => account.Transactions)
                   .Include(account => account.User)
                   .Select(account => new UserInfoReturnModel()
                    {
                       CustomerId = account.User.UserId,
                       AccountId = account.AccountId,
                       FirstName = account.User.FirstName,
                       SureName = account.User.SureName,
                       Balance = account.Balance,
                       Transactions = account.Transactions.Select(accountTransactions => new TransactionReturnModel()
                       {
                           TransactionId =  accountTransactions.TransactionId,
                           TransactionType = accountTransactions.TransactionType
                       }).ToList()

                    })
                   .ToListAsync();

        }

        private async Task<User> GetUser(string UserId)
        {
            return await _dbContext
                         .Users
                         .Where(user => user.UserId == UserId).FirstOrDefaultAsync();
        }
    }
}
