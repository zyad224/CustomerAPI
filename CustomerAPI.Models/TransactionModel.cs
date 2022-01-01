using CustomerAPI.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAPI.Models
{
    public class TransactionModel
    {
        public string UserId { get; set; }
        public string AccountId { get; set; }
        public string Amount { get; set; }
    }
}
