using MediatR;
using OutboxPattern.Domain.Events;
using OutboxPattern.Domain.Models;
using OutboxPattern.Domain.Repositories;

namespace OutboxPattern.Application.DomainEventHandlers
{
    internal class OrderPlacedDomainEventHandler : INotificationHandler<OrderPlacedDomainEvent>
    {
        private readonly IProductRepository _productRepository;

        public OrderPlacedDomainEventHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(OrderPlacedDomainEvent notification, CancellationToken cancellationToken)
        {
            Product product = await _productRepository.GetProductAsync(notification.ProductId);

            product.DecreaseQuantity();
        }
    }
}
