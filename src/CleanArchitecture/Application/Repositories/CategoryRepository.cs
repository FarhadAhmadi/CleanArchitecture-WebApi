using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;

namespace CleanArchitecture.Application.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly DapperContext _dapperContext;

        // Constructor to inject both EF Core context and Dapper context
        public CategoryRepository(ApplicationDbContext context, DapperContext dapperContext)
            : base(context, dapperContext)
        {
            _dapperContext = dapperContext;
        }
    }
} 
