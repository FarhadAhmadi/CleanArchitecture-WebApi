using CleanArchitecture.Shared.Models.Author.DTOs;
using CleanArchitecture.Shared.Models.Author.Requests;
using CleanArchitecture.Shared.Models.Book.DTOs;
using CleanArchitecture.Shared.Models.Book.Requests;
using CleanArchitecture.Shared.Models.Response;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IAuthorService
{
    Task<Pagination<AuthorDTO>> Get(AuthorSearchRequest request);
    Task<AuthorDTO> Get(string id);
    Task<AuthorDTO> Add(CreateAuthorRequest request, CancellationToken token);
    Task<AuthorDTO> Update(UpdateAuthorRequest request, CancellationToken token);
    Task<bool> Delete(string id, CancellationToken token);
}
