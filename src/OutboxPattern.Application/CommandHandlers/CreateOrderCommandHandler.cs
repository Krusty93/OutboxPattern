using MediatR;
using OutboxPattern.Application.Commands;
using OutboxPattern.Domain.Models;
using OutboxPattern.Domain.Repositories;

namespace OutboxPattern.Application.CommandHandlers
{
    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductAsync(request.ProductId);

            var order = Order.Create(
                product.Price,
                request.Quantity,
                request.ShippingAddress.ToString(),
                request.ProductId);

            await _orderRepository.CreateOrderAsync(order);

            order.ExecuteOrder();

            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return order.Id;
        }
    }
}
