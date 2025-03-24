using PersonalTasksProject.DTOs.Requests;

namespace PersonalTasksProject.DTOs.Responses;

public class PaginatedResponse<T>  where T : class 
{
    public Pagination Pagination { get; set; }
    public List<T> Data { get; set; }

    public PaginatedResponse(Pagination pagination, List<T> data)
    {
        Pagination = pagination;
        Data = data;
    }
}