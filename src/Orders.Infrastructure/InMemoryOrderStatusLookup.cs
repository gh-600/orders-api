using Orders.Application;
using Orders.Domain;

namespace Orders.Infrastructure;

public sealed class InMemoryOrderStatusLookup : IOrderStatusLookup
{
    private static readonly IReadOnlyDictionary<Guid, OrderStatus> OrderStatuses =
        new Dictionary<Guid, OrderStatus>
        {
            [Guid.Parse("11111111-1111-1111-1111-111111111111")] = OrderStatus.Created
        };

    public OrderStatus? FindStatus(Guid orderId) =>
        OrderStatuses.TryGetValue(orderId, out var status) ? status : null;
}
