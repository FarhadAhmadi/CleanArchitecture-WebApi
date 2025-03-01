using CleanArchitecture.Shared.Models;

namespace CleanArchitecture.Domain.Entities;

public class Order : BaseModel
{
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
