using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CustomerAPI.Models
{
    public class UserInfoModel
    {
        public string CustomerId { get; set; }
        public decimal InitialCredit { get; set; }
        public string FirstName { get; set; }
        public string SureName { get; set; }

    }
}
