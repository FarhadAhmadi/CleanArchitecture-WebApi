using CleanArchitecture.Shared.Models;

namespace CleanArchitecture.Domain.Entities;

public class Publisher : BaseModel
{
    public string Name { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
