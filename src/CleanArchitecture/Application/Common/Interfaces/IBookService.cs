using CleanArchitecture.Shared.Models.Book.DTOs;
using CleanArchitecture.Shared.Models.Book.Requests;
using CleanArchitecture.Shared.Models.Response;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IBookService
{
    Task<Pagination<BookDTO>> Get(BookSearchRequest request);
    Task<BookDTO> Get(string id);
    Task<BookDTO> Add(CreateBookRequest request, CancellationToken token);
    Task<BookDTO> Update(UpdateBookRequest request, CancellationToken token);
    Task<bool> Delete(string id, CancellationToken token);
}
