using CleanArchitecture.Shared.Models.Base;

namespace CleanArchitecture.Domain.Entities;

public class ForgotPassword : BaseEntity
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public string OTP { get; set; }
    public DateTime DateTime { get; set; }
}
