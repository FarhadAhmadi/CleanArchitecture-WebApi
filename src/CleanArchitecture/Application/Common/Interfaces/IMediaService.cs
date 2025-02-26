using CleanArchitecture.Shared.Models.AuthIdentity.Media;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IMediaService
{
    Task RemoveMediaAsync(string mediaId, CancellationToken cancellationToken);
    Task UpdateMediaAsync(string mediaId, MediaCreateRequest request, CancellationToken cancellationToken);
}
