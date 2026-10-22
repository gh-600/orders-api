using Orders.Domain;

namespace Orders.Application;

public interface IOrderStatusLookup
{
    OrderStatus? FindStatus(Guid orderId);
}
