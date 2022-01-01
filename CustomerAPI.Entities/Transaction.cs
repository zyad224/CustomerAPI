using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAPI.Entities
{
    public class Transaction: EntityBase
    {
        public long Id { get; set; }
        public string TransactionId { get; set; }
        public TransactionType TransactionType { get; set; }
        public Account Account { get; set; }
    }

   public enum TransactionType
    {
        CheckBalance = 0,
        Deposit = 1,
        Withdraw = 2

    }
}
