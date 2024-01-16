using Crud.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Crud.Infrastructure.DbContext
{
    public class CrudContext : Microsoft.EntityFrameworkCore.DbContext
    {

        public CrudContext(DbContextOptions<CrudContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(e => e.ToTable("tblCustomers"));
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
