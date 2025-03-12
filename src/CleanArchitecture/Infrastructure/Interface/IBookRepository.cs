using CleanArchitecture.Shared.Models.Book.DTOs;
using CleanArchitecture.Shared.Models.Book.Requests;
using CleanArchitecture.Shared.Models.Response;

namespace CleanArchitecture.Infrastructure.Interface;

public interface IBookRepository : IGenericRepository<Book>
{
}
