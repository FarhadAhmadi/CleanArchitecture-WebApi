using CleanArchitecture.Shared.Models.Base;

namespace CleanArchitecture.Domain.Entities;
public class RefreshToken : BaseEntity
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
