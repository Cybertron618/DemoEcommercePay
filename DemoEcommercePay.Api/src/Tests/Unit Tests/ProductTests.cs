using System;
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
    public class ProductTests
    {
        private readonly Mock<EcommerceContext>? _mockContext;
        private readonly IProductRepository _productRepository;

        public ProductTests(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ProductTests()
        {
            // Initialize the mock context and repository
            _mockContext = new Mock<EcommerceContext>();
            _productRepository = new ProductRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetProductsAsync_ReturnsProductsList()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = 1, Name = "Product 1", Price = 10.99m },
                new() { Id = 2, Name = "Product 2", Price = 20.99m }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Product>>();
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            _mockContext!.Setup(m => m.Products).Returns(mockDbSet.Object);

            // Act
            var result = await _productRepository.GetProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // Assuming we have 2 products in the mock context
        }

        // Add more test like GetProductByIdAsync, CreateProductAsync, UpdateProductAsync, and DeleteProductAsync
    }
}
