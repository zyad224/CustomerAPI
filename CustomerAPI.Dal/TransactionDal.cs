using CustomerAPI.Dal.Interfaces;
using CustomerAPI.Entities;
using CustomerAPI.Models;
using System;
using System.Collections.Generic;
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
            var userAccount = new Account() { UserId = transactionModel.UserId };
            
            var accountTransaction = new Transaction() { Account = userAccount };

            Decimal.TryParse(transactionModel.Amount, out decimal amountDecimal);

            accountTransaction.TransactionType = GetTransactionType(amountDecimal);
            userAccount.Balance = GetBalance(userAccount.Balance, amountDecimal, accountTransaction.TransactionType);
        
            userAccount.Transactions.Add(accountTransaction);

            await _dbContext.Accounts.AddAsync(userAccount);

            return await _dbContext.SaveChangesAsync() > 0;
                    
        }

        private TransactionType GetTransactionType(decimal amountDecimal)
        {
            return amountDecimal > 0 ? TransactionType.Deposit 
                : amountDecimal < 0 ? TransactionType.Withdraw
                : TransactionType.CheckBalance;
        }

        private decimal GetBalance(decimal currentBalance, decimal amountDecimal, TransactionType transactionType)
        {

            if(transactionType == TransactionType.Deposit)
            {
                currentBalance += amountDecimal;
            }
            else if(transactionType == TransactionType.Withdraw)
            {
                currentBalance -= (-1 * amountDecimal);
            }
            return currentBalance;
        }
    }
}
