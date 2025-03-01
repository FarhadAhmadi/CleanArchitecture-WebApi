namespace CleanArchitecture.Shared.Models.Book;

public class BookDTO : BaseDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double? Price { get; set; }

}
public class BookEditDTO : BaseDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double? Price { get; set; }

    public AuthorDTO Author { get; set; } = new AuthorDTO();
    public PublisherDTO Publisher { get; set; } = new PublisherDTO();
    public List<ReviewDTO> Reviews { get; set; } = new List<ReviewDTO>();
    public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

}

public class AddBookRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double? Price { get; set; }
}
public class UpdateBookRequest
{
    public string Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double? Price { get; set; }
}
