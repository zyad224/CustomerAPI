using CustomerAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.Dal.Interfaces
{
    public interface ITransactionDal
    {
        public Task<bool> OpenNewAccountExistingUserTransaction(TransactionModel transactionModel);

    }
}
