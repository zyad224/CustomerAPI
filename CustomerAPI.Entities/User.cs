﻿using CustomerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAPI.Entities
{
    public class User: EntityBase
    {
        public long Id { get; set; }
        public string UserId { get; set; } = UUIDGenerator.GetNewUUID();
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
