using Microsoft.EntityFrameworkCore;
using DemoEcommercePay.Api.src.Domain.Entities;

namespace DemoEcommercePay.Api.src.Infrastructure.Data
{
    public class EcommerceContext(DbContextOptions<EcommerceContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Your entity mappings here
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Order>().ToTable("Orders");
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}

