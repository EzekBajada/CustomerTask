using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CustomerTask.Models
{
    public class CustomerContext: DbContext
    {
        public CustomerContext(DbContextOptions options) : base(options) { }

        public DbSet<Customers> Customers { get; set; }
        
        public DbSet<CustomerDetails> CustomerDetails { get; set; }
    }
}
