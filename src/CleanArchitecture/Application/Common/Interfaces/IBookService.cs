using CleanArchitecture.Shared.Models;
using CleanArchitecture.Shared.Models.Book;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IBookService
{
    Task<Pagination<BookDTO>> Get(SearchRequest request);
    Task<BookDTO> Get(string id);
    Task<BookDTO> Add(AddBookRequest request, CancellationToken token);
    Task<BookDTO> Update(UpdateBookRequest request, CancellationToken token);
    Task<BookDTO> Delete(string id, CancellationToken token);
}
