using System.Text.Json;

namespace CleanArchitecture.Domain;
public record Error(string? Code, string Message, string ErrorId)
{
    public static implicit operator string(Error error) => JsonSerializer.Serialize(error);
};
