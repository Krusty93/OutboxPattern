using OutboxPattern.Domain.Models;
using OutboxPattern.Domain.Repositories;

namespace OutboxPattern.Infrastructure.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly OutboxDbContext _dbContext;

        public ProductRepository(
            OutboxDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public async Task CreateProductAsync(Product product)
        {
            await _dbContext.AddAsync(product);
        }

        public void DeleteProduct(Product product)
        {
            _dbContext.Remove(product);
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            return product;
        }
    }
}
