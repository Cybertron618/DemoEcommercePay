using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEcommercePay.Api.src.Domain.Entities;



namespace DemoEcommercePay.Api.src.Infrastructure.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }
}
