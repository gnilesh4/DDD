using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DomainDrivenDesign.Tests
{
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
}
