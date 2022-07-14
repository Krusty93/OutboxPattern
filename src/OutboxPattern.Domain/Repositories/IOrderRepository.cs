using OutboxPattern.Domain.Models;

namespace OutboxPattern.Domain.Repositories
{
    public interface IOrderRepository : IRepository
    {
        Task CreateOrderAsync(Order order);

        Task<Order> GetOrderAsync(Guid id);

        void DeleteOrder(Order order);
    }
}
