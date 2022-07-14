using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OutboxPattern.Infrastructure
{
    public abstract class BaseDbContext : DbContext
    {
        private readonly string _timeStampFieldName;

        protected BaseDbContext(DbContextOptions options, string timeStampFieldName) : base(options)
        {
            _timeStampFieldName = timeStampFieldName;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_timeStampFieldName != null)
            {
                var entries = ChangeTracker.Entries();

                foreach (var entry in entries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                        case EntityState.Modified:

                            if (entry.CurrentValues.EntityType.FindProperty(_timeStampFieldName) != null)
                            {
                                entry.CurrentValues[_timeStampFieldName] = DateTime.UtcNow;
                            }
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
