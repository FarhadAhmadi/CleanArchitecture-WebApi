using CleanArchitecture.Shared.Models.Publisher.DTOs;
using CleanArchitecture.Shared.Models.Publisher.Requests;
using CleanArchitecture.Shared.Models.Response;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IPublisherService
{
    Task<Pagination<PublisherDTO>> Get(PublisherSearchRequest request);
    Task<PublisherDTO> Get(string id);
    Task<PublisherDTO> Add(CreatePublisherRequest request, CancellationToken token);
    Task<PublisherDTO> Update(UpdatePublisherRequest request, CancellationToken token);
    Task<bool> Delete(string id, CancellationToken token);
}
