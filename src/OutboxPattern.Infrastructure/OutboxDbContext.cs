using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OutboxPattern.Domain.Models;
using OutboxPattern.Domain.Repositories;
using OutboxPattern.Infrastructure.Data;

namespace OutboxPattern.Infrastructure
{
    public class OutboxDbContext : BaseDbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public OutboxDbContext(
            DbContextOptions<OutboxDbContext> opt,
            IMediator mediator)
            : base(opt, timeStampFieldName: "TimeStampUTC")
        {
            _mediator = mediator;
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new OrderDataConfiguration());
            modelBuilder.ApplyConfiguration(new ProductDataConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this);

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

            return new OutboxDbContext(optionsBuilder.Options, null);
        }
    }
}
