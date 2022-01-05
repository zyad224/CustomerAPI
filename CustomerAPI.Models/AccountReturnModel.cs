using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAPI.Models
{
    public class AccountReturnModel
    {

        public string AccountId { get; set; } 
        public string UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public List<TransactionReturnModel> Transactions = new List<TransactionReturnModel>();
    }
}
