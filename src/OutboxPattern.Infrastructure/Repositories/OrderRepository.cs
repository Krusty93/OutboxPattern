using OutboxPattern.Domain.Models;
using OutboxPattern.Domain.Repositories;

namespace OutboxPattern.Infrastructure.Repositories
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly OutboxDbContext _dbContext;

        public OrderRepository(
            OutboxDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public async Task CreateOrderAsync(Order order)
        {
            await _dbContext.AddAsync(order);
        }

        public void DeleteOrder(Order order)
        {
            _dbContext.Remove(order);
        }

        public async Task<Order> GetOrderAsync(Guid id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            return order;
        }
    }
}
