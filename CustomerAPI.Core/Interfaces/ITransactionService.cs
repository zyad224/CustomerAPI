using CustomerAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.Core.Interfaces
{
    public interface ITransactionService
    {
        public Task<TransactionReturnModel> DepositTransaction(TransactionModel transactionModel);
        public Task<TransactionReturnModel> WithDrawTransaction(TransactionModel transactionModel);
        public Task<TransactionReturnModel> CheckBalanceTransaction(TransactionModel transactionModel);
    }
}
