using Dapper;
using OutboxPattern.Application.Models;
using OutboxPattern.Domain.Models;
using OutboxPattern.Infrastructure;

namespace OutboxPattern.Application.Queries
{
    public interface IProductQueries
    {
        Task<AllProductResponse> GetAllProductsAsync();

        Task<ProductResponse> GetProductAsync(Guid id);
    }

    internal class ProductQueries : IProductQueries
    {
        private readonly DapperContext _context;

        public ProductQueries(DapperContext context)
        {
            _context = context;
        }

        public async Task<AllProductResponse> GetAllProductsAsync()
        {
            const string QUERY = "SELECT * FROM [dbo].[Products]";

            using var connection = _context.CreateConnection();

            var orders = await connection.QueryAsync<Product>(QUERY);

            var dto = orders.
                Select(order => new ProductResponse
                {
                    Id = order.Id,
                    Name = order.Name,
                    Price = order.Price,
                    Quantity = order.Quantity,
                });

            return new AllProductResponse(dto);
        }

        public async Task<ProductResponse> GetProductAsync(Guid id)
        {
            const string QUERY = "SELECT * FROM [dbo].[Products] WHERE Id = @Id";
            using var connection = _context.CreateConnection();

            try
            {
                var product = await connection.QuerySingleAsync<Product>(QUERY, new { Id = id });

                return new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity,
                };
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
