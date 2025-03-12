using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Shared.Models.Author.DTOs;
using CleanArchitecture.Shared.Models.Author.Requests;
using CleanArchitecture.Shared.Models.Publisher.DTOs;
using CleanArchitecture.Shared.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Services;

public class AuthorService : IAuthorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    public AuthorService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
    }
    public async Task<Pagination<AuthorDTO>> Get(AuthorSearchRequest request)
    {
        var authors = await _unitOfWork.AuthorRepository.ToPagination<AuthorDTO>(
        pageIndex: request.PageIndex,
        pageSize: request.PageSize,
        filter: null,
        include: null,
        orderBy: b => b.Id,
        ascending: true,
        selector: x => new AuthorDTO
        {
            Id = x.Id,
            Bio = x.Bio,
            Name = x.Name,
            CreatedOn = x.CreatedOn,
            CreatorId = x.CreatorId,
            UpdatedOn = x.UpdatedOn,
            UpdaterId = x.UpdaterId,
        });

        return authors;
    }

    public async Task<AuthorDTO> Get(string id)
    {
        var author = await _unitOfWork.AuthorRepository.GetSingleData<AuthorDTO>(
        filter: p => p.Id == id,   // Filter by Id
        include: null,
        orderBy: p => p.Id,  // Order by Name, if needed
        ascending: true,  // Ascending order
        selector: x => new AuthorDTO
        {
            Id = x.Id,
            Bio = x.Bio,
            Name = x.Name,
            CreatedOn = x.CreatedOn,
            CreatorId = x.CreatorId,
            UpdatedOn = x.UpdatedOn,
            UpdaterId = x.UpdaterId,
        });

        return author;
    }
    public async Task<AuthorDTO> Add(CreateAuthorRequest request, CancellationToken token)
    {
        var author = _mapper.Map<Author>(request);
        author.CreatedOn = DateTime.UtcNow;
        author.CreatorId = _currentUser.GetCurrentUserId();


        await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.AuthorRepository.AddAsync(author), token);

        return _mapper.Map<AuthorDTO>(author);
    }
    public async Task<AuthorDTO> Update(UpdateAuthorRequest request, CancellationToken token)
    {
        var author = await _unitOfWork.AuthorRepository.GetByIdAsyncWithDapper(DatabaseTableNames.Author, request.Id);

        if (author == null)
            throw new UserFriendlyException(ErrorCode.NotFound, "Author not found");

        // Only update the fields that are provided in the request
        if (!string.IsNullOrEmpty(request.Name)) author.Name = request.Name;
        if (!string.IsNullOrEmpty(request.Bio)) author.Bio = request.Bio;

        // Update timestamp and user information
        author.UpdatedOn = DateTimeOffset.UtcNow;
        author.UpdaterId = _currentUser.GetCurrentUserId();

        // Perform the update within a transaction
        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.AuthorRepository.Update(author), token);

        // Return the updated Author as DTO
        return _mapper.Map<AuthorDTO>(author);
    }
    public async Task<bool> Delete(string id, CancellationToken token)
    {
        var author = await _unitOfWork.AuthorRepository.GetByIdAsyncWithDapper(DatabaseTableNames.Author, id)
            ?? throw new UserFriendlyException(ErrorCode.NotFound, "Author not found");

        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.AuthorRepository.Delete(author), token);
        return true;
    }


}
