using CustomerAPI.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAPI.Models
{
    public class UserInfoReturnModel
    {
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string SureName { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public List<AccountReturnModel> Accounts = new List<AccountReturnModel>();

    }
}
