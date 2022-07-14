using System.Diagnostics.CodeAnalysis;

namespace OutboxPattern.Domain.Models
{
    public class Product : Entity<Guid>, IAggregateRoot
    {
        // Reseved to EF
        [ExcludeFromCodeCoverage]
        protected Product()
        {
        }

        private Product(string name, decimal price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public int Quantity { get; private set; }

        public static Product Create(
            string name,
            decimal price,
            int quantity)
        {
            return new Product(name, price, quantity);
        }

        public void DecreaseQuantity()
        {
            Quantity--;
        }
    }
}
