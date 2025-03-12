using CleanArchitecture.Shared.Models.Author.DTOs;
using CleanArchitecture.Shared.Models.Base;
using CleanArchitecture.Shared.Models.Publisher.DTOs;

namespace CleanArchitecture.Shared.Models.Book.DTOs;

public class BookEditDTO : BaseDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double? Price { get; set; }

    public AuthorDTO Author { get; set; } = new AuthorDTO();
    public PublisherDTO Publisher { get; set; } = new PublisherDTO();
    //public List<ReviewDTO> Reviews { get; set; } = new List<ReviewDTO>();
    //public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

}

