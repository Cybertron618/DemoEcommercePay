using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEcommercePay.Api.src.Domain.Entities;

namespace DemoEcommercePay.Api.src.Infrastructure.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
