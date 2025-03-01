using System.Linq.Expressions;
using CleanArchitecture.Shared.Models;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Dapper;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Application.Common;
using Microsoft.Data.SqlClient;
using CleanArchitecture.Application.Common.Utilities;
using System.Text;

namespace CleanArchitecture.Application.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly DbSet<T> _dbSet;
    private readonly DapperContext _dapperContext;

    public GenericRepository(ApplicationDbContext context, DapperContext dapperContext)
    {
        _dbSet = context.Set<T>();
        _dapperContext = dapperContext ?? throw new ArgumentNullException(nameof(dapperContext)); // Ensure it's not null
    }

    #region Dapper Methods

    public async Task<Pagination<TResult>> ToPagination<T, TResult>(
        string tableName,
        int pageIndex,
        int pageSize,
        Expression<Func<T, TResult>> selector,
        string? orderByColumn = "Id",
        bool ascending = true) where T : BaseModel
    {
        using var connection = _dapperContext.CreateConnection();

        var selectedColumns = ExtractSelectedColumns(selector);

        var sql = BuildPaginationQuery(tableName, selectedColumns, orderByColumn, ascending);

        var parameters = CreateParameters(pageIndex, pageSize);

        var result = (await connection.QueryAsync<TResult, int, (TResult, int)>(
            sql,
            (data, totalCount) => (data, totalCount),
            parameters,
            splitOn: "TotalCount"
        )).ToArray(); // Convert to array

        return MapToPagination<TResult>(result, pageIndex, pageSize);

    }

    #region ToPagination Private Methods

    private List<string> ExtractSelectedColumns<T, TResult>(Expression<Func<T, TResult>> selector)
    {
        return ExtracterHelper.ExtractSelectedColumns(selector);
    }

    private string BuildPaginationQuery(string tableName, List<string> selectedColumns, string orderByColumn, bool ascending)
    {
        string sortDirection = ascending ? "ASC" : "DESC";

        return $@"
        WITH FilteredData AS (
            SELECT {string.Join(", ", selectedColumns)}, COUNT(*) OVER() AS TotalCount
            FROM {tableName}
        )
        SELECT {string.Join(", ", selectedColumns)}, TotalCount FROM FilteredData
        ORDER BY {orderByColumn} {sortDirection}
        OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;";
    }


    private DynamicParameters CreateParameters(int pageIndex, int pageSize)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PageIndex", pageIndex);
        parameters.Add("@PageSize", pageSize);

        return parameters;
    }

    private Pagination<TResult> MapToPagination<TResult>((TResult, int)[] result, int pageIndex, int pageSize)
    {
        var items = result.Select(r => r.Item1).ToList();
        int totalCount = result.Any() ? result.First().Item2 : 0;
        return new Pagination<TResult>(items, totalCount, pageIndex, pageSize);
    }


    #endregion 

    public async Task<T> GetByIdAsync(string tableName, object id)
    {
        using var connection = _dapperContext.CreateConnection();

        string sql = $"SELECT * FROM {tableName} WHERE Id = @Id";

        return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
    }

    public async Task<TResult?> GetByIdAsync<T, TResult>(
        string tableName,
        object id,
        Expression<Func<T, TResult>> selector)
    {
        using var connection = _dapperContext.CreateConnection();

        // Extract selected columns from the selector expression
        var selectedColumns = ExtracterHelper.ExtractSelectedColumns(selector);

        // Generate the column selection dynamically
        string selectedColumnsSql = string.Join(", ", selectedColumns);

        // SQL query
        string sql = $"SELECT {selectedColumnsSql} FROM {tableName} WHERE Id = @Id";

        return await connection.QueryFirstOrDefaultAsync<TResult>(sql, new { Id = id });
    }

    public async Task<bool> ExistsAsync(string tableName, string keyColumn, object value)
    {
        using var connection = _dapperContext.CreateConnection();
        string sql = $"SELECT COUNT(1) FROM {tableName} WHERE {keyColumn} = @Value";

        return await connection.ExecuteScalarAsync<int>(sql, new { Value = value }) > 0;
    }


    #endregion

    #region EF Core Methods

    #region Add

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    #endregion

    #region  Read

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbSet.AnyAsync(filter);
    }

    public async Task<bool> AnyAsync()
    {
        return await _dbSet.AnyAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
    {
        return filter == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(filter);
    }

    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<T> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<Pagination<TResult>> ToPagination<TResult>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        Expression<Func<T, TResult>> selector = null)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        if (include != null)
        {
            query = include(query);
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        orderBy ??= x => EF.Property<object>(x, "Id");

        query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

        var projectedQuery = query.Select(selector);

        var result = await Pagination<TResult>.ToPagedList(projectedQuery, pageIndex, pageSize);

        return result;
    }

    public async Task<T?> FirstOrDefaultAsync(
    Expression<Func<T, bool>> filter,
    Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        IQueryable<T> query = _dbSet.IgnoreQueryFilters().AsNoTracking();

        if (include != null)
        {
            query = include(query);
        }

        return await query.FirstOrDefaultAsync(filter);
    }

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter,
        Expression<Func<T, object>> sort, bool ascending = true)
    {
        var query = _dbSet.IgnoreQueryFilters()
                          .AsNoTracking()
                          .Where(filter);

        query = ascending ? query.OrderBy(sort) : query.OrderByDescending(sort);

        return await query.FirstOrDefaultAsync();
    }

    #endregion

    #region Update & delete

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public async Task Delete(object id)
    {
        T entity = await GetByIdAsync(id);
        Delete(entity);
    }

    #endregion

    #endregion
}
