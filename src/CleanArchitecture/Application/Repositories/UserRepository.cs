using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;

namespace CleanArchitecture.Application.Repositories;

public class UserRepository(ApplicationDbContext context, DapperContext dapperContext) : GenericRepository<User>(context, dapperContext), IUserRepository { }
