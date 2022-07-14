using Dapper;
using OutboxPattern.Application.Models;
using OutboxPattern.Domain.Models;
using OutboxPattern.Infrastructure;

namespace OutboxPattern.Application.Queries
{
    public interface IOrderQueries
    {
        Task<AllOrdersResponse> GetAllOrdersAsync();

        Task<OrderResponse> GetOrderAsync(Guid id);
    }

    internal class OrderQueries : IOrderQueries
    {
        private readonly DapperContext _context;

        public OrderQueries(DapperContext context)
        {
            _context = context;
        }

        public async Task<AllOrdersResponse> GetAllOrdersAsync()
        {
            const string QUERY = "SELECT * FROM [dbo].[Orders]";

            using var connection = _context.CreateConnection();

            var orders = await connection.QueryAsync<Order>(QUERY);

            var dto = orders.
                Select(order => new OrderResponse
                {
                    Id = order.Id,
                    Total = order.Total,
                    ShippingAddress = order.ShippingAddress,
                });

            return new AllOrdersResponse(dto);
        }

        public async Task<OrderResponse> GetOrderAsync(Guid id)
        {
            const string QUERY = "SELECT * FROM [dbo].[Orders] WHERE Id = @Id";
            using var connection = _context.CreateConnection();

            try
            {
                var order = await connection.QuerySingleAsync<Order>(QUERY, new { Id = id });

                return new OrderResponse
                {
                    Id = order.Id,
                    Total = order.Total,
                    ShippingAddress = order.ShippingAddress,
                };
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
