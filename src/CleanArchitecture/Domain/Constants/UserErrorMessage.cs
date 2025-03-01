namespace CleanArchitecture.Domain.Constants;

public static class UserErrorMessage
{
    public const string AlreadyExists = "{0} already exists!";
    public const string Unauthorized = "User is not logged in.";
    public const string UserNotExist = "The specified user does not exist.";
    public const string PasswordIncorrect = "The password entered is incorrect.";
}

public static class DatabaseTableNames
{
    public static readonly Dictionary<string, string> Tables = new()
    {
        { nameof(Book), "Books" }
    };

    public const string Book = "Books";
    public const string Category = "Categories";
    public const string Author = "Authors";
    public const string Publisher = "Publishers";
}
