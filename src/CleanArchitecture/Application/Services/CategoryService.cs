using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Shared.Models.Author.DTOs;
using CleanArchitecture.Shared.Models.Category.DTOs;
using CleanArchitecture.Shared.Models.Category.Requests;
using CleanArchitecture.Shared.Models.Publisher.DTOs;
using CleanArchitecture.Shared.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<Pagination<CategoryDTO>> Get(CategorySearchRequest request)
    {
        var categories = await _unitOfWork.CategoryRepository.ToPagination<CategoryDTO>(
            pageIndex: request.PageIndex,
            pageSize: request.PageSize,
            filter: null,
            include: null,
            orderBy: b => b.Id,
            ascending: true,
            selector: x => new CategoryDTO
            {
                Id = x.Id,
                Name = x.Name,
                CreatedOn = x.CreatedOn,
                CreatorId = x.CreatorId,
                UpdatedOn = x.UpdatedOn,
                UpdaterId = x.UpdaterId,
            });

        return categories;
    }

    public async Task<CategoryDTO> Get(string id)
    {
        var category = await _unitOfWork.CategoryRepository.GetSingleData<CategoryDTO>(
            filter: p => p.Id == id,   // Filter by Id
            include: null,
            orderBy: p => p.Id,  // Order by Name, if needed
            ascending: true,  // Ascending order
            selector: x => new CategoryDTO
            {
                Id = x.Id,
                Name = x.Name,
                CreatedOn = x.CreatedOn,
                CreatorId = x.CreatorId,
                UpdatedOn = x.UpdatedOn,
                UpdaterId = x.UpdaterId,
            });

        return category;
    }

    public async Task<CategoryDTO> Add(CreateCategoryRequest request, CancellationToken token)
    {
        var category = _mapper.Map<Category>(request);
        category.CreatedOn = DateTime.UtcNow;
        category.CreatorId = _currentUser.GetCurrentUserId();

        await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.CategoryRepository.AddAsync(category), token);

        return _mapper.Map<CategoryDTO>(category);
    }

    public async Task<CategoryDTO> Update(UpdateCategoryRequest request, CancellationToken token)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsyncWithDapper(DatabaseTableNames.Category, request.Id);

        if (category == null)
            throw new UserFriendlyException(ErrorCode.NotFound, "Category not found");

        // Only update the fields that are provided in the request
        if (!string.IsNullOrEmpty(request.Name)) category.Name = request.Name;
        
        // Update timestamp and user information
        category.UpdatedOn = DateTimeOffset.UtcNow;
        category.UpdaterId = _currentUser.GetCurrentUserId();
        
        // Perform the update within a transaction
        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.CategoryRepository.Update(category), token);

        // Return the updated category as DTO
        return _mapper.Map<CategoryDTO>(category);
    }

    public async Task<bool> Delete(string id, CancellationToken token)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsyncWithDapper(DatabaseTableNames.Category, id)
            ?? throw new UserFriendlyException(ErrorCode.NotFound, "Category not found");

        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.CategoryRepository.Delete(category), token);
        return true;
    }
}
