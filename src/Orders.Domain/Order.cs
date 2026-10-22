namespace Orders.Domain;

public enum OrderStatus
{
    Created,
    Processing,
    Completed
}

public sealed class Order
{
    public Guid Id { get; } = Guid.NewGuid();
    public decimal Amount { get; }
    public OrderStatus Status { get; } = OrderStatus.Created;

    public Order(decimal amount) => Amount = amount;
}
