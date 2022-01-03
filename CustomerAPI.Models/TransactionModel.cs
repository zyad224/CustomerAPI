using CustomerAPI.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAPI.Models
{
    public class TransactionModel
    {
        public string CustomerId { get; set; }
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
