using Microsoft.EntityFrameworkCore;
using PersonalTasksProject.DTOs.Requests;

namespace PersonalTasksProject.Helpers;

public class PagedList<T> where T : class
{
    public Pagination Pagination { get; set; }
    public List<T> Data { get; private set; }

    public PagedList(List<T> data, int count, int pageSize, int pageNumber)
    {
        Pagination = new Pagination(count, pageSize, pageNumber);
        Data = data;
    }

    public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        
        return new PagedList<T>(items, count, pageSize, pageNumber);
    }
}