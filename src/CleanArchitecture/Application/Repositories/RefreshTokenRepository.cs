using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;

namespace CleanArchitecture.Application.Repositories;

public class RefreshTokenRepository(ApplicationDbContext context, DapperContext dapperContext) : GenericRepository<RefreshToken>(context, dapperContext), IRefreshTokenRepository { }
