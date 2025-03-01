using CleanArchitecture.Shared.Models;

namespace CleanArchitecture.Domain.Entities;

public class Author : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
