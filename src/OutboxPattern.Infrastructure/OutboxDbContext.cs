using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OutboxPattern.Domain.Models;
using OutboxPattern.Domain.Repositories;
using OutboxPattern.Infrastructure.Data;
using OutboxPattern.Infrastructure.Processing;

namespace OutboxPattern.Infrastructure
{
    public class OutboxDbContext : BaseDbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;

        public OutboxDbContext(
            DbContextOptions<OutboxDbContext> opt,
            IMediator mediator,
            IServiceProvider serviceProvider)
            : base(opt, timeStampFieldName: "TimeStampUTC")
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new OrderDataConfiguration());
            modelBuilder.ApplyConfiguration(new ProductDataConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this, _serviceProvider);

            int result = await base.SaveChangesAsync(cancellationToken);

            return result >= 0;
        }
    }

    public class DesignTimeOutboxDbContext : IDesignTimeDbContextFactory<OutboxDbContext>
    {
        public OutboxDbContext CreateDbContext(string[] args)
        {
            var cs = args[0];

            var optionsBuilder = new DbContextOptionsBuilder<OutboxDbContext>();
            optionsBuilder.UseSqlServer(cs);

            return new OutboxDbContext(optionsBuilder.Options, null, null);
        }
    }
}
