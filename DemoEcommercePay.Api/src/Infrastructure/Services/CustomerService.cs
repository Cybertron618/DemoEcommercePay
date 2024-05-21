using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEcommercePay.Api.src.Infrastructure.Repositories;
using DemoEcommercePay.Api.src.Infrastructure.Services;
using DemoEcommercePay.Api.src.Domain.Entities;

namespace DemoEcommercePay.Api.src.Infrastructure.Services
{
    public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _customerRepository.GetCustomersAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetCustomerByIdAsync(id);
        }

        public async Task CreateCustomerAsync(Customer customer)
        {
            await _customerRepository.CreateCustomerAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _customerRepository.UpdateCustomerAsync(customer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            await _customerRepository.DeleteCustomerAsync(id);
        }
    }
}

