using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEcommercePay.Api.src.Domain.Entities;
using DemoEcommercePay.Api.src.Infrastructure.Data;
using DemoEcommercePay.Api.src.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DemoEcommercePay.Api.src.Tests.IntegrationTests
{
    public class OrderTests
    {
        private readonly DbContextOptions<EcommerceContext> _options;

        public OrderTests()
        {
            _options = new DbContextOptionsBuilder<EcommerceContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldAddOrderToDatabase()
        {
            // Arrange
            using (var context = new EcommerceContext(_options))
            {
                var repository = new OrderRepository(context);
                var order = new Order
                {
                    Id = 1,
                    CustomerId = 1,
                };

                // Act
                await repository.CreateOrderAsync(order);
            }

            // Assert
            using (var context = new EcommerceContext(_options))
            {
                var orders = await context.Orders.ToListAsync();
                Assert.Single(orders); 
            }
        }

    }
}
