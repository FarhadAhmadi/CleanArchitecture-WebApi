using CleanArchitecture.Shared.Models.Category.DTOs;
using CleanArchitecture.Shared.Models.Category.Requests;
using CleanArchitecture.Shared.Models.Response;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface ICategoryService
{
    Task<Pagination<CategoryDTO>> Get(CategorySearchRequest request);
    Task<CategoryDTO> Get(string id);
    Task<CategoryDTO> Add(CreateCategoryRequest request, CancellationToken token);
    Task<CategoryDTO> Update(UpdateCategoryRequest request, CancellationToken token);
    Task<bool> Delete(string id, CancellationToken token);
}
