namespace OutboxPattern.Domain.Repositories
{
    public interface IRepository
    {
        public IUnitOfWork UnitOfWork { get; }
    }
}
