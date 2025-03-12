using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Shared.Models.Publisher.DTOs;
using CleanArchitecture.Shared.Models.Publisher.Requests;
using CleanArchitecture.Shared.Models.Response;

namespace CleanArchitecture.Application.Services;

public class PublisherService : IPublisherService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    public PublisherService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<Pagination<PublisherDTO>> Get(PublisherSearchRequest request)
    {
        var publishers = await _unitOfWork.PublisherRepository.ToPagination<PublisherDTO>(
            pageIndex: request.PageIndex,
            pageSize: request.PageSize,
            filter: null,
            include: null,
            orderBy: b => b.Id,
            ascending: true,
            selector: x => new PublisherDTO
            {
                Id = x.Id,
                Name = x.Name,
                CreatedOn = x.CreatedOn,
                CreatorId = x.CreatorId,
                UpdatedOn = x.UpdatedOn,
                UpdaterId = x.UpdaterId,
            });

        return publishers;
    }

    public async Task<PublisherDTO> Get(string id)
    {
        var publisher = await _unitOfWork.PublisherRepository.GetSingleData<PublisherDTO>(
            filter: p => p.Id == id,   // Filter by Id
            include: null,
            orderBy: p => p.Id,  // Order by Name, if needed
            ascending: true,  // Ascending order
            selector: x => new PublisherDTO
            {
                Id = x.Id,
                CreatedOn = x.CreatedOn,
                CreatorId = x.CreatorId,
                UpdatedOn = x.UpdatedOn,
                UpdaterId = x.UpdaterId,
            });

        return publisher;
    }

    public async Task<PublisherDTO> Add(CreatePublisherRequest request, CancellationToken token)
    {
        var publisher = _mapper.Map<Publisher>(request);
        publisher.CreatedOn = DateTime.UtcNow;
        publisher.CreatorId = _currentUser.GetCurrentUserId();

        await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.PublisherRepository.AddAsync(publisher), token);

        return _mapper.Map<PublisherDTO>(publisher);
    }

    public async Task<PublisherDTO> Update(UpdatePublisherRequest request, CancellationToken token)
    {
        var publisher = await _unitOfWork.PublisherRepository.GetByIdAsyncWithDapper(DatabaseTableNames.Publisher, request.Id);

        if (publisher == null)
            throw new UserFriendlyException(ErrorCode.NotFound , "Publisher not found");

        // Only update the fields that are provided in the request
        if (!string.IsNullOrEmpty(request.Name)) publisher.Name = request.Name;

        // Update timestamp and user information
        publisher.UpdatedOn = DateTimeOffset.UtcNow;
        publisher.UpdaterId = _currentUser.GetCurrentUserId();

        // Perform the update within a transaction
        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.PublisherRepository.Update(publisher), token);

        // Return the updated publisher as DTO
        return _mapper.Map<PublisherDTO>(publisher);
    }

    public async Task<bool> Delete(string id, CancellationToken token)
    {
        var publisher = await _unitOfWork.PublisherRepository.GetByIdAsyncWithDapper(DatabaseTableNames.Publisher, id)
            ?? throw new UserFriendlyException(ErrorCode.NotFound, "Publisher not found");

        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.PublisherRepository.Delete(publisher), token);
        return true;
    }
}
