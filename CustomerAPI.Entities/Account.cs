using CustomerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAPI.Entities
{
    public class Account : EntityBase
    {
        public long Id { get; set; }
        public string AccountId { get; set; } = UUIDGenerator.GetNewUUID();
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string SureName { get; set; }
        public decimal Balance { get; set; }
        public User User { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
