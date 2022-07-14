using Microsoft.Extensions.DependencyInjection;
using OutboxPattern.Domain.Repositories;
using OutboxPattern.Infrastructure.Repositories;

namespace OutboxPattern.Infrastructure
{
    public static class Bootstrapper
    {
        public static void InitializeInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
