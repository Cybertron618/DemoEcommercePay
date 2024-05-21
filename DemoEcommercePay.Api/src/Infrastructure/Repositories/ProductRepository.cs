using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEcommercePay.Api.src.Domain.Entities;
using DemoEcommercePay.Api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoEcommercePay.Api.src.Infrastructure.Repositories
{
    public class ProductRepository(EcommerceContext context) : IProductRepository
    {
        private readonly EcommerceContext _context = context;

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id) ?? throw new InvalidOperationException("Product not found");
            return product!;
        }


        public async Task CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
