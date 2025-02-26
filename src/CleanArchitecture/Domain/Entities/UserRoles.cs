using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domain.Entities;

public class UserRoles : IdentityUserRole<string>
{
    public virtual ApplicationUser User { get; set; }

    public virtual RoleIdentity Role { get; set; }
}
