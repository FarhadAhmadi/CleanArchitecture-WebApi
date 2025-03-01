using CleanArchitecture.Shared.Models;

namespace CleanArchitecture.Domain.Entities;

public class Category : BaseModel
{
    public string Name { get; set; } = string.Empty;

    // Many-to-Many Relationship
    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
}
