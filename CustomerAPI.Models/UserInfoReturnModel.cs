using CustomerAPI.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAPI.Models
{
    public class UserInfoReturnModel
    {
        public string CustomerId { get; set; }
        public string AccountId { get; set; }
        public string FirstName { get; set; }
        public string SureName { get; set; }
        public decimal Balance { get; set; }
        public List<TransactionReturnModel> Transactions { get; set; } = new List<TransactionReturnModel>();

    }
}
