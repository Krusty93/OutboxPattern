using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutboxPattern.Domain.Models;

namespace OutboxPattern.Infrastructure.Data
{
    internal class OrderDataConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Ignore(x => x.DomainEvents);

            builder.HasKey(o => o.Id);
            builder
                .Property(o => o.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
        }
    }
}
