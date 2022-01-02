using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAPI.Models
{
    public class UserInfoModel
    {
        public string CustomerId { get; set; }
        public string InitialCredit { get; set; }
        public string FirstName { get; set; }
        public string SureName { get; set; }
    }
}
