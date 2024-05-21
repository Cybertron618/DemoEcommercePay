using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEcommercePay.Api.src.Domain.Entities;
using DemoEcommercePay.Api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoEcommercePay.Api.src.Infrastructure.Repositories
{
    public class CustomerRepository(EcommerceContext context) : ICustomerRepository
    {
        private readonly EcommerceContext _context = context;

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            return customer ?? throw new InvalidOperationException("Customer not found");
        }


        public async Task CreateCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
