using MediatR;
using OutboxPattern.Application.Commands;
using OutboxPattern.Domain.Models;
using OutboxPattern.Domain.Repositories;

namespace OutboxPattern.Application.CommandHandlers
{
    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repository;

        public CreateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var order = Product.Create(request.Name, request.Price, request.Quantity);

            await _repository.CreateProductAsync(order);

            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return order.Id;
        }
    }
}
