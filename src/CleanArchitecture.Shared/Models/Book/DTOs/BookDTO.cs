using CleanArchitecture.Shared.Models.Author.DTOs;
using CleanArchitecture.Shared.Models.Base;
using CleanArchitecture.Shared.Models.Publisher.DTOs;

namespace CleanArchitecture.Shared.Models.Book.DTOs;

public class BookDTO : BaseDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double? Price { get; set; }

    public string AuthorId { get; set; }
    public AuthorDTO Author { get; set; } = new AuthorDTO();
    public string PublisherId { get; set; }
    public PublisherDTO Publisher { get; set; } = new PublisherDTO();

}
