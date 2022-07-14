using Microsoft.Extensions.DependencyInjection;
using OutboxPattern.Application.Queries;

namespace OutboxPattern.Application
{
    public static class Bootstrapper
    {
        public static void InitializeApplication(this IServiceCollection services)
        {
            services.AddScoped<IOrderQueries, OrderQueries>();
            services.AddScoped<IProductQueries, ProductQueries>();
        }
    }
}
