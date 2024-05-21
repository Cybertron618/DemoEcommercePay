using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoEcommercePay.Api.src.Domain.Entities;
using DemoEcommercePay.Api.src.Infrastructure.Data;
using DemoEcommercePay.Api.src.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DemoEcommercePay.Api.src.Tests.UnitTests
{
    public class CustomerTests
    {
        private readonly Mock<EcommerceContext>? _mockContext;
        private readonly ICustomerRepository _customerRepository;

        public CustomerTests(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public CustomerTests()
        {
            // Initialize the mock context and repository
            _mockContext = new Mock<EcommerceContext>();
            _customerRepository = new CustomerRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetCustomersAsync_ReturnsCustomersList()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new() { Id = 1, Name = "Customer 1" },
                new() { Id = 2, Name = "Customer 2" }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Customer>>();
            mockDbSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(customers.Provider);
            mockDbSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.Expression);
            mockDbSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.ElementType);
            mockDbSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(customers.GetEnumerator());

            _mockContext!.Setup(m => m.Customers).Returns(mockDbSet.Object);

            // Act
            var result = await _customerRepository.GetCustomersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // Assuming we have 2 customers in the mock context
        }

        // Add more test methods for other repository methods like GetCustomerByIdAsync, CreateCustomerAsync, UpdateCustomerAsync, and DeleteCustomerAsync
    }
}
