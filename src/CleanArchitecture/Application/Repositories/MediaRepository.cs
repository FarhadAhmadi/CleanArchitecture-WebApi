using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;

namespace CleanArchitecture.Application.Repositories;

public class MediaRepository(ApplicationDbContext context, DapperContext dapperContext) : GenericRepository<Media>(context, dapperContext), IMediaRepository { }
