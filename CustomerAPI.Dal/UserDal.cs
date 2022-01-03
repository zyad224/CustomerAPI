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
        private DbApiContext _dbContext;

        public UserDal(DbApiContext dbApiContext)
        {
            _dbContext = dbApiContext;
        }
        public async Task<UserInfoReturnModel> AddUser(UserInfoModel userInfoModel)
        {
            var user = new User { FirstName = userInfoModel.FirstName, SureName = userInfoModel.SureName };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return new UserInfoReturnModel() { CustomerId = user.UserId, FirstName = user.FirstName, SureName = user.SureName, CreateOn = user.CreateOn, ModifiedOn = user.ModifiedOn };

        }

        public async Task<UserInfoReturnModel> UserInfo(UserInfoModel userInfoModel)
        {
          
            return await _dbContext.Users
                           .Include(user => user.Accounts)
                           .ThenInclude(account => account.Transactions)
                           .Where(user => user.UserId == userInfoModel.CustomerId)
                           .Select(model => new UserInfoReturnModel()
                           {
                               CustomerId = model.UserId,
                               FirstName = model.FirstName,
                               SureName = model.SureName,
                               CreateOn = model.CreateOn,
                               ModifiedOn = model.ModifiedOn,
                               Accounts = model.Accounts.Select(account => new AccountReturnModel()
                               {
                                   AccountId = account.AccountId,
                                   UserId = account.User.UserId,
                                   Balance = account.Balance,
                                   CreateOn = account.CreateOn,
                                   ModifiedOn = account.ModifiedOn,
                                   Transactions = account.Transactions.Select(transaction => new TransactionReturnModel()
                                   {
                                       TransactionId = transaction.TransactionId,
                                       TransactionType = transaction.TransactionType,
                                       CreateOn = transaction.CreateOn,
                                       ModifiedOn = transaction.ModifiedOn

                                   }).ToList()

                               }).ToList()
                              
                           }).FirstOrDefaultAsync();
        
        }
    }
}
