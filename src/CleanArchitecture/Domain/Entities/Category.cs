using CleanArchitecture.Shared.Models.Base;

namespace CleanArchitecture.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }

    // Many-to-Many Relationship
    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
}
