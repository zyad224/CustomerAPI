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
        CheckBalance = 0,
        Deposit = 1,
        Withdraw = 2

    }
}
