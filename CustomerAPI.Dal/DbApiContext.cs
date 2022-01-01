using CustomerAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace CustomerAPI.Dal
{
    public class DbApiContext: DbContext
    {
        public DbApiContext(DbContextOptions<DbApiContext> options)
           : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts{ get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}
