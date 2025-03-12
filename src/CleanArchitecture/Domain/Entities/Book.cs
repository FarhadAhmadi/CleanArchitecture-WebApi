using CleanArchitecture.Shared.Models.Base;

namespace CleanArchitecture.Domain.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; } 
    public double Price { get; set; }

    // Foreign Keys
    public string AuthorId { get; set; }
    public Author Author { get; set; }

    public string PublisherId { get; set; }
    public Publisher Publisher { get; set; }

    // Navigation Properties
    public ICollection<BookCategory> BookCategories { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}
