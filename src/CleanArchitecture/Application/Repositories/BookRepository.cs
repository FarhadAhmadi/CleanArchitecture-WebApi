using System.Text;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;
using CleanArchitecture.Shared.Models.Book;
using CleanArchitecture.Shared.Models.Book.DTOs;
using CleanArchitecture.Shared.Models.Book.Requests;
using CleanArchitecture.Shared.Models.Response;
using Dapper;
using k8s;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

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
    }
} 
