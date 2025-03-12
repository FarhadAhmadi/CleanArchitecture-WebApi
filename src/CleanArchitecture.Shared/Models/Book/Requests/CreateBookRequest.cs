using CleanArchitecture.Shared.Models.Base;

namespace CleanArchitecture.Shared.Models.Book.Requests;

public class CreateBookRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double? Price { get; set; }


    public string AuthorId { get; set; }
    public string PublisherId { get; set; }
}
