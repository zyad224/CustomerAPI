using CustomerAPI.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAPI.Models
{
    public class TransactionReturnModel
    {
        public string TransactionId { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
