using CustomerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CustomerAPI.Entities
{
    public class Transaction : EntityBase
    {
        
        public long Id { get; set; }
        [Key]
        public string TransactionId { get; set; } = UUIDGenerator.GetNewUUID();
        public TransactionType TransactionType { get; set; }
        [ForeignKey("AccountId")]
        public string AccountId{ get; set; }
        public Account Account { get; set; }
    }

   public enum TransactionType
    {
        NoTransaction = 0,
        CheckBalance = 1,
        Deposit = 2,
        Withdraw = 3,
    }
}
