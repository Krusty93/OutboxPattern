using OutboxPattern.Domain.Models;

namespace OutboxPattern.Domain.Repositories
{
    public interface IProductRepository : IRepository
    {
        Task CreateProductAsync(Product product);

        Task<Product> GetProductAsync(Guid id);

        void DeleteProduct(Product product);
    }
}
