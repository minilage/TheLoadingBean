using TheLoadingBean.Shared.Models;

namespace TheLoadingBeanAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(string id);
        Task<List<Order>> GetOrdersByCustomerIdAsync(string customerId);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(string id, Order order);
        Task<bool> DeleteOrderAsync(string id);
    }
}
