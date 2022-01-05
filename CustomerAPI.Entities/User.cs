using CustomerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CustomerAPI.Entities
{
    public class User: EntityBase
    {
        public long Id { get; set; }
        [Key]
        public string UserId { get; set; } = UUIDGenerator.GetNewUUID();
        public string FirstName { get; set; }
        public string SureName { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}
