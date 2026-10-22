using Orders.Domain;

namespace Orders.Tests;

public class OrderTests
{
    [Fact]
    public void New_order_keeps_amount()
    {
        var order = new Order(42m);
        Assert.Equal(42m, order.Amount);
        Assert.Equal(OrderStatus.Created, order.Status);
    }
}
