namespace CleanArchitecture.Shared.Models.User;

public class UserSignUpResponse
{
    public string Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
}
