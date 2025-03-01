using CleanArchitecture.Shared.Models;

namespace CleanArchitecture.Domain.Entities;

public class Book : BaseModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double? Price { get; set; }

    // Foreign Keys
    public required string AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public string PublisherId { get; set; }
    public Publisher Publisher { get; set; } = null!;

    // Navigation Properties
    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
