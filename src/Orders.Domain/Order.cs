namespace Orders.Domain;

public sealed class Order
{
    public Guid Id { get; } = Guid.NewGuid();
    public decimal Amount { get; }

    public Order(decimal amount) => Amount = amount;
}
