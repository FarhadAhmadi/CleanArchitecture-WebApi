using CleanArchitecture.Shared.Models.Book;

namespace CleanArchitecture.Infrastructure.Interface;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<BookEditDTO> GetBookWithDetails(string id);
}
