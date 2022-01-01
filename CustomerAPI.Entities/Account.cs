using CustomerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CustomerAPI.Entities
{
    public class Account : EntityBase
    {
        public long Id { get; set; }
        [Key]
        public string AccountId { get; set; } = UUIDGenerator.GetNewUUID();
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string SureName { get; set; }
        public decimal Balance { get; set; }
        public User User { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
