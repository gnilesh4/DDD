using System;

namespace DomainDrivenDesign
{
    public sealed class Product : Entity
    {
        public Product
        (
            string description,
            Amount price
        )
        {
            ChangeDescription(description);
            ChangePrice(price);
        }

        public string Description { get; private set; }

        public Amount Price { get; private set; }

        public void ChangeDescription(string description)
        {
            ValidateDescription(description);
            Description = description;
        }

        public void ChangePrice(Amount price)
        {
            ValidatePrice(price);
            Price = price;
        }

        private void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException(nameof(description));
            }
        }

        private void ValidatePrice(Amount price)
        {
            if (price == null || price.Value <= 0)
            {
                throw new ArgumentException(nameof(price));
            }
        }
    }
}
