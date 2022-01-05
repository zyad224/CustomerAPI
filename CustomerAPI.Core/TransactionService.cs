using CustomerAPI.Core.Interfaces;
using CustomerAPI.Dal.Interfaces;
using CustomerAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.Core
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionDal _transactionDal;

        public TransactionService(ITransactionDal transactionDal)
        {
            _transactionDal = transactionDal;
        }
        public async Task<TransactionReturnModel> CheckBalanceTransaction(TransactionModel transactionModel)
        {
            return await _transactionDal.CheckBalanceTransaction(transactionModel);
        }

        public async Task<TransactionReturnModel> DepositTransaction(TransactionModel transactionModel)
        {
            return await _transactionDal.DepositTransaction(transactionModel);
        }

        public async Task<TransactionReturnModel> WithDrawTransaction(TransactionModel transactionModel)
        {
            return await _transactionDal.WithDrawTransaction(transactionModel);
        }
    }
}
