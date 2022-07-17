using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutboxPattern.Infrastructure.Processing;

namespace OutboxPattern.Infrastructure.Data
{
    internal class OutboxDataConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");

            builder.HasKey(x => x.Id);
            builder
                .Property(o => o.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
        }
    }
}
