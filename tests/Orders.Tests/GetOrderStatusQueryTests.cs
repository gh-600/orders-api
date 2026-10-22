using Orders.Application;
using Orders.Domain;

namespace Orders.Tests;

public class GetOrderStatusQueryTests
{
    private static readonly Guid OrderId = Guid.NewGuid();

    [Fact]
    public void Returns_status_for_existing_order()
    {
        var handler = new GetOrderStatusQueryHandler(new StubOrderStatusLookup(OrderStatus.Processing));

        var result = handler.Handle(new GetOrderStatusQuery(OrderId));

        Assert.Equal(new GetOrderStatusResult(OrderId, OrderStatus.Processing), result);
    }

    [Fact]
    public void Returns_null_for_unknown_order()
    {
        var handler = new GetOrderStatusQueryHandler(new StubOrderStatusLookup(null));

        var result = handler.Handle(new GetOrderStatusQuery(OrderId));

        Assert.Null(result);
    }

    private sealed class StubOrderStatusLookup(OrderStatus? status) : IOrderStatusLookup
    {
        public OrderStatus? FindStatus(Guid orderId) => status;
    }
}
