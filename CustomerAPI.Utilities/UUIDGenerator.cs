using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAPI.Utilities
{
    public static class UUIDGenerator
    {
        public static string GetNewUUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
