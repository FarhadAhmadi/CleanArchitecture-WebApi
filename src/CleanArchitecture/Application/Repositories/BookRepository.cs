using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;
using CleanArchitecture.Shared.Models.Book;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly DapperContext _dapperContext;

        // Constructor to inject both EF Core context and Dapper context
        public BookRepository(ApplicationDbContext context, DapperContext dapperContext)
            : base(context, dapperContext)
        {
            _dapperContext = dapperContext;
        }

        // Method to get a book with its related data using Dapper
        public async Task<BookEditDTO> GetBookWithDetails(string id)
        {
            using var connection = _dapperContext.CreateConnection();

            var sql = $@"
        SELECT 
            book.Id,
            book.Title,
            book.Description,
            book.Price, 
            book.AuthorId,
            book.PublisherId, 
            author.Id AS AuthorId,
            author.Name AS AuthorName,
            author.Bio AS AuthorBio,
            publisher.Id AS PublisherId,
            publisher.Name AS PublisherName
        FROM {DatabaseTableNames.Book} book
        INNER JOIN {DatabaseTableNames.Author} author ON book.AuthorId = author.Id
        LEFT JOIN {DatabaseTableNames.Publisher} publisher ON book.PublisherId = publisher.Id
        WHERE book.Id = @Id;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var result = await connection.QueryAsync<Book, Author, Publisher, BookEditDTO>(
                sql,
                (book, author, publisher) =>
                {
                    // Map the result to BookEditDTO
                    var bookEditDto = new BookEditDTO
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Description = book.Description,
                        Price = book.Price,
                        Author = new AuthorDTO
                        {
                            Id = author.Id,
                            Name = author.Name,
                            Bio = author.Bio,
                            CreatedOn = author.CreatedOn,
                            CreatorId = author.CreatorId,
                            UpdatedOn = author.UpdatedOn,
                            UpdaterId = author.UpdaterId
                        },
                        Publisher = new PublisherDTO
                        {
                            Id = publisher.Id,
                            Name = publisher.Name,
                            CreatedOn = publisher.CreatedOn,
                            CreatorId = publisher.CreatorId,
                            UpdatedOn = publisher.UpdatedOn,
                            UpdaterId = publisher.UpdaterId
                        }
                    };

                    return bookEditDto;
                },
                parameters,
                splitOn: "AuthorId,PublisherId" // Split the result based on AuthorId and PublisherId
            );

            return result.SingleOrDefault();
        }

    }
}
