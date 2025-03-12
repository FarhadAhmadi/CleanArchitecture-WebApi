using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;

namespace CleanArchitecture.Application.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly DapperContext _dapperContext;

        // Constructor to inject both EF Core context and Dapper context
        public AuthorRepository(ApplicationDbContext context, DapperContext dapperContext)
            : base(context, dapperContext)
        {
            _dapperContext = dapperContext;
        }
    }
} 
