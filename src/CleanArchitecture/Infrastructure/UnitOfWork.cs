using CleanArchitecture.Application;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Repositories;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private readonly DapperContext _dapperContext;

    public IUserRepository UserRepository { get; }
    public IBookRepository BookRepository { get; }
    public IRefreshTokenRepository RefreshTokenRepository { get; }
    public IMediaRepository MediaRepository { get; }
    public IForgotPasswordRepository ForgotPasswordRepository { get; }

    public UnitOfWork(ApplicationDbContext dbContext , DapperContext dapperContext)
    {
        _context = dbContext;
        _dapperContext = dapperContext;
        UserRepository = new UserRepository(_context, _dapperContext);
        BookRepository = new BookRepository(_context, _dapperContext);
        RefreshTokenRepository = new RefreshTokenRepository(_context, _dapperContext);
        MediaRepository = new MediaRepository(_context, _dapperContext);
        ForgotPasswordRepository = new ForgotPasswordRepository(_context, _dapperContext);
    }
    public async Task SaveChangesAsync(CancellationToken token)
        => await _context.SaveChangesAsync(token);

    public async Task ExecuteTransactionAsync(Action action, CancellationToken token)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(token);
        try
        {
            action();
            await _context.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            throw TransactionException.TransactionNotExecuteException(ex);
        }
    }

    public async Task ExecuteTransactionAsync(Func<Task> action, CancellationToken token)
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await _context.Database.BeginTransactionAsync(token);
            try
            {
                await action();
                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(token);
                throw TransactionException.TransactionNotExecuteException(ex);
            }
        });
    }

}
