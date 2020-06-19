# Domain-Driven Design

Domain-Driven Design is a software development approach in which it utilizes concepts and good practices related to object-oriented programming.

## Books

Domain-Driven Design: Tackling Complexity in the Heart of Software - Eric Evans | Implementing Domain-Driven Design - Vaughn Vernon
:-------------------------:|:-------------------------:
![](https://dddcommunity.org/wp-content/uploads/files/images/cover_medium.jpg) | ![](https://dddcommunity.org/wp-content/uploads/2013/02/implementing-domain-driven-design-400x400-imae6dr5trk3uycd.jpeg)





## Tests

```cs
[TestClass]
public class Tests
{
    [TestMethod]
    public void Main()
    {
        var customer = CustomerFactory.Create("Luke", "Skywalker", "luke.skywalker@starwars.com");

        var product = ProductFactory.Create("Millennium Falcon", 500_000_000);

        var item = OrderItemFactory.Create(product, 1);

        var order = OrderFactory.Create(customer);

        order.AddItem(item);

        var discount = new DiscountService().Calculate(order.Total, DiscountType.Large);

        order.ApplyDiscount(discount);

        Assert.AreEqual(250_000_000, order.Total.Value);
    }
}
```

## Customer

```cs
public sealed class Customer : Entity
{
    public Customer(FullName fullName, Email email)
    {
        FullName = fullName;
        Email = email;
    }

    public Email Email { get; private set; }

    public FullName FullName { get; private set; }

    public void ChangeEmail(Email email)
    {
        Email = email;
    }

    public void ChangeName(FullName fullName)
    {
        FullName = fullName;
    }
}
```

```cs
public static class CustomerFactory
{
    public static Customer Create(string firstName, string lastName, string email)
    {
        return new Customer(new FullName(firstName, lastName), new Email(email));
    }
}
```

## Product

```cs
public sealed class Product : Entity
{
    public Product(string description, Amount price)
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
```

```cs
public static class ProductFactory
{
    public static Product Create(string description, decimal price)
    {
        return new Product(description, new Amount(price));
    }
}
```

## Order

```cs
public sealed class Order : Entity
{
    public Order(Customer customer)
    {
        Customer = customer;
        Discount = new Amount(0);
        Items = new List<OrderItem>();
    }

    public Customer Customer { get; private set; }

    public Amount Discount { get; private set; }

    public IReadOnlyList<OrderItem> Items { get; private set; }

    public Amount Total => new Amount(Items.Sum(item => item.SubTotal.Value) - Discount.Value);

    public void AddItem(OrderItem item)
    {
        Items = new List<OrderItem>(Items) { item };
    }

    public void ApplyDiscount(Amount discount)
    {
        Discount = discount;
    }
}
```

```cs
public static class OrderFactory
{
    public static Order Create(Customer customer)
    {
        return new Order(customer);
    }
}
```

## Order Item

```cs
public sealed class OrderItem : Entity
{
    public OrderItem(Product product, Quantity quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public Product Product { get; private set; }

    public Quantity Quantity { get; private set; }

    public Amount SubTotal => new Amount(Product.Price.Value * Quantity.Value);
}
```

```cs
public static class OrderItemFactory
{
    public static OrderItem Create(Product product, decimal quantity)
    {
        return new OrderItem(product, new Quantity(quantity));
    }
}
```

## Discount

```cs
public enum DiscountType
{
    Small = 1,
    Medium = 2,
    Large = 3
}
```

```cs
public sealed class DiscountService
{
    public Amount Calculate(Amount amount, DiscountType type)
    {
        var discount = Factory.Get<IDiscount>(x => x.IsApplicable(type));

        if (discount == null) { return amount; }

        return discount.Calculate(amount);
    }
}
```

```cs
public interface IDiscount
{
    Amount Calculate(Amount amount);

    bool IsApplicable(DiscountType type);
}
```

```cs
public sealed class SmallDiscount : IDiscount
{
    public Amount Calculate(Amount amount)
    {
        return new Amount(amount.Value * 0.1M);
    }

    public bool IsApplicable(DiscountType type) => type == DiscountType.Small;
}
```

```cs
public sealed class MediumDiscount : IDiscount
{
    public Amount Calculate(Amount amount)
    {
        return new Amount(amount.Value * 0.25M);
    }

    public bool IsApplicable(DiscountType type) => type == DiscountType.Medium;
}
```

```cs
public sealed class LargeDiscount : IDiscount
{
    public Amount Calculate(Amount amount)
    {
        return new Amount(amount.Value * 0.5M);
    }

    public bool IsApplicable(DiscountType type) => type == DiscountType.Large;
}
```

## Value Objects

```cs
public sealed class Amount : ValueObject
{
    public Amount(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; private set; }

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetEquals()
    {
        yield return Value;
    }
}
```

```cs
public sealed class Email : ValueObject
{
    public Email(string value)
    {
        Validate(value);
        Value = value;
    }

    public string Value { get; private set; }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetEquals()
    {
        yield return Value;
    }

    private void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(nameof(value));
        }

        const string regex = @"^([a-z0-9_\.\-]{3,})@([\da-z\.\-]{3,})\.([a-z\.]{2,6})$";

        if (!new Regex(regex).IsMatch(value))
        {
            throw new ArgumentException(nameof(value));
        }
    }
}
```

```cs
public sealed class FullName : ValueObject
{
    public FullName(string firstName, string lastName)
    {
        FirstName = firstName ?? throw new ArgumentException(nameof(firstName));

        LastName = lastName ?? throw new ArgumentException(nameof(lastName));
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public override string ToString() => $"{ FirstName } { LastName }";

    protected override IEnumerable<object> GetEquals()
    {
        yield return FirstName;
        yield return LastName;
    }
}
```

```cs
public sealed class Quantity : ValueObject
{
    public Quantity(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; private set; }

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetEquals()
    {
        yield return Value;
    }
}
```
