namespace CleanArchitecture.Domain.Entities;

public class OrderItem
{
    public string OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public string BookId { get; set; }
    public Book Book { get; set; } = null!;

    public int Quantity { get; set; }
}
