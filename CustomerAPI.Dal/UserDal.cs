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
    public class UserDal : IUserDal
    {
        private readonly DbApiContext _dbContext;

        public UserDal(DbApiContext dbApiContext)
        {
            _dbContext = dbApiContext;
        }
        public async Task<UserInfoReturnModel> AddUser(UserInfoModel userInfoModel)
        {
            var user = new User { FirstName = userInfoModel.FirstName, SureName = userInfoModel.SureName };

            await _dbContext.Users.AddAsync(user);

            return new UserInfoReturnModel() { CustomerId = user.UserId, FirstName = user.FirstName, SureName = user.SureName };

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
                          TransactionId = accountTransactions.TransactionId,
                          TransactionType = accountTransactions.TransactionType
                      }).ToList()

                  })
                  .ToListAsync();
        }
    }
}
