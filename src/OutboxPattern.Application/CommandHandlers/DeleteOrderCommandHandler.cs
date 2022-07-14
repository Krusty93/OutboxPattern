using MediatR;
using OutboxPattern.Application.Commands;
using OutboxPattern.Domain.Repositories;

namespace OutboxPattern.Application.CommandHandlers
{
    internal class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _repository;

        public DeleteOrderCommandHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetOrderAsync(request.OrderId);

            _repository.DeleteOrder(order);

            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
