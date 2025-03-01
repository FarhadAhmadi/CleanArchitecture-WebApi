public class BookCategory
{
    public string BookId { get; set; }
    public Book Book { get; set; } = null!;

    public string CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
