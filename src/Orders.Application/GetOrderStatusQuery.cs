using Orders.Domain;

namespace Orders.Application;

public sealed record GetOrderStatusQuery(Guid OrderId);

public sealed record GetOrderStatusResult(Guid Id, OrderStatus Status);

public sealed class GetOrderStatusQueryHandler(IOrderStatusLookup orderStatusLookup)
{
    public GetOrderStatusResult? Handle(GetOrderStatusQuery query)
    {
        var status = orderStatusLookup.FindStatus(query.OrderId);
        return status is null ? null : new GetOrderStatusResult(query.OrderId, status.Value);
    }
}
