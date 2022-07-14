using MediatR;
using OutboxPattern.Application.Commands;
using OutboxPattern.Domain.Repositories;

namespace OutboxPattern.Application.CommandHandlers
{
    internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _repository;

        public DeleteProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProductAsync(request.ProductId);

            _repository.DeleteProduct(product);

            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
