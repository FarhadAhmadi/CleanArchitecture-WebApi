using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shared.Models.Response;

public class Pagination<T>(List<T> items, int count, int pageIndex, int pageSize)
{
    public int CurrentPage { get; set; } = pageIndex;
    public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = count;
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    public List<T>? Items { get; set; } = items;

    public static async Task<Pagination<T>> ToPagedList(IQueryable<T> source, int pageIndex, int pageSize)
    {
        pageIndex = pageIndex <= 0 ? 1 : pageIndex;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var count = source.Count();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new Pagination<T>(items, count, pageIndex, pageSize);
    }
    public static async Task<Pagination<T>> ToPagedList(List<T> source, int pageIndex, int pageSize)
    {
        pageIndex = pageIndex <= 0 ? 1 : pageIndex;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var count = source.Count();
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new Pagination<T>(items, count, pageIndex, pageSize);
    }
}
