namespace CleanArchitecture.Shared.Models;

public abstract class BaseModel
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public DateTimeOffset CreatedOn { get; init; } = DateTimeOffset.UtcNow;
    public string? CreatorId { get; init; }

    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdaterId { get; set; }
}
