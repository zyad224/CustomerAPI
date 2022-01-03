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
    public class TransactionDal : ITransactionDal
    {
        private readonly DbApiContext _dbContext;

        public TransactionDal(DbApiContext dbApiContext)
        {
            _dbContext = dbApiContext;
        }

        public async Task<bool> OpenNewAccountExistingUserTransaction(TransactionModel transactionModel)
        {
            var userAccount = new Account() { UserId = transactionModel.CustomerId };
         
            if(GetTransactionTypeByAmount(transactionModel.Amount)!=TransactionType.NoTransaction)
            {
                var accountTransaction = new Transaction() { Account = userAccount };
                accountTransaction.TransactionType = GetTransactionTypeByAmount(transactionModel.Amount);
                userAccount.Balance = GetBalance(userAccount.Balance, transactionModel.Amount, accountTransaction.TransactionType);
                userAccount.Transactions.Add(accountTransaction);

            }

            await _dbContext.Accounts.AddAsync(userAccount);

            return await _dbContext.SaveChangesAsync() > 0;
                              
        }

      
        public async Task<TransactionReturnModel> DepositTransaction(TransactionModel transactionModel)
        {
            var userAccount = await _dbContext.Accounts
                              .Where(account => (account.AccountId == transactionModel.AccountId) &&
                              (account.User.UserId == transactionModel.CustomerId))
                              .FirstOrDefaultAsync();

            TransactionReturnModel transactionReturnModel = new TransactionReturnModel();
            Transaction accountTransaction = new Transaction();
            if (userAccount!=null)
            {
                accountTransaction.Account = userAccount;
                accountTransaction.TransactionType = TransactionType.Deposit;
                userAccount.Balance = GetBalance(userAccount.Balance, transactionModel.Amount, TransactionType.Deposit);
                userAccount.Transactions.Add(accountTransaction);

            }

           if( await _dbContext.SaveChangesAsync()>0)
            {
                transactionReturnModel.TransactionId = accountTransaction.TransactionId;
                transactionReturnModel.TransactionType = TransactionType.Deposit;
                transactionReturnModel.CreateOn = accountTransaction.CreateOn;
                transactionReturnModel.ModifiedOn = accountTransaction.ModifiedOn;
                
            }

            return transactionReturnModel;
          
        }

        public async Task<TransactionReturnModel> WithDrawTransaction(TransactionModel transactionModel)
        {
            var userAccount = await _dbContext.Accounts
                             .Where(account => (account.AccountId == transactionModel.AccountId) &&
                             (account.User.UserId == transactionModel.CustomerId))
                             .FirstOrDefaultAsync();

            TransactionReturnModel transactionReturnModel = new TransactionReturnModel();
            Transaction accountTransaction = new Transaction();

            if (userAccount.Balance <= 0)
                return transactionReturnModel;

            
            if (userAccount != null)
            {
                accountTransaction.Account = userAccount;
                accountTransaction.TransactionType = TransactionType.Withdraw;
                userAccount.Balance = GetBalance(userAccount.Balance, transactionModel.Amount, TransactionType.Withdraw);
                userAccount.Transactions.Add(accountTransaction);

            }

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                transactionReturnModel.TransactionId = accountTransaction.TransactionId;
                transactionReturnModel.TransactionType = TransactionType.Withdraw;
                transactionReturnModel.CreateOn = accountTransaction.CreateOn;
                transactionReturnModel.ModifiedOn = accountTransaction.ModifiedOn;
            }

            return transactionReturnModel;
        }

        public async Task<TransactionReturnModel> CheckBalanceTransaction(TransactionModel transactionModel)
        {
            var userAccount = await _dbContext.Accounts
                            .Where(account => (account.AccountId == transactionModel.AccountId) &&
                            (account.User.UserId == transactionModel.CustomerId))
                            .FirstOrDefaultAsync();

            TransactionReturnModel transactionReturnModel = new TransactionReturnModel();
            Transaction accountTransaction = new Transaction();

            if (userAccount != null)
            {
                accountTransaction.Account = userAccount;
                accountTransaction.TransactionType = TransactionType.CheckBalance;
                userAccount.Transactions.Add(accountTransaction);

            }

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                transactionReturnModel.TransactionId = accountTransaction.TransactionId;
                transactionReturnModel.TransactionType = TransactionType.CheckBalance;
                transactionReturnModel.CreateOn = accountTransaction.CreateOn;
                transactionReturnModel.ModifiedOn = accountTransaction.ModifiedOn;
            }

            return transactionReturnModel;
        }

        private TransactionType GetTransactionTypeByAmount(decimal amount)
        {
            return amount > 0 ? TransactionType.Deposit
                : amount < 0 ? TransactionType.Withdraw
                : TransactionType.NoTransaction;
        }

        private decimal GetBalance(decimal currentBalance, decimal amountDecimal, TransactionType transactionType)
        {
            decimal newBalance = 0;

            if (transactionType == TransactionType.Deposit)
            {
                newBalance = currentBalance + amountDecimal;
            }
            else if (transactionType == TransactionType.Withdraw)
            {
                newBalance = currentBalance - (-1 * amountDecimal);
            }
            return newBalance;
        }
    }
}
