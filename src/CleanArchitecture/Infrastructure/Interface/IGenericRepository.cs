using System.Linq.Expressions;
using CleanArchitecture.Shared.Models;

namespace CleanArchitecture.Infrastructure.Interface;

public interface IGenericRepository<T> where T : class
{

    #region Dapper

    Task<Pagination<TResult>> ToPagination<T, TResult>(
    string tableName,
    int pageIndex,
    int pageSize,
    Expression<Func<T, TResult>> selector,
    string? orderByColumn = "Id",
    bool ascending = true) where T : BaseModel;

    Task<T> GetByIdAsync(string tableName, object id);
    Task<TResult> GetByIdAsync<T, TResult>(
        string tableName,
        object id, Expression<Func<T, TResult>> selector);

    Task<bool> ExistsAsync(string tableName, string keyColumn, object value);

    #endregion

    #region EF Core 

    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    Task<bool> AnyAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> filter);
    Task<int> CountAsync();
    Task<T> GetByIdAsync(object id);
    Task<Pagination<TResult>> ToPagination<TResult>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        Expression<Func<T, TResult>> selector = null);
    Task<T?> FirstOrDefaultAsync(
       Expression<Func<T, bool>> filter,
       Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> sort, bool ascending = true);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    Task Delete(object id);

    #endregion
}
