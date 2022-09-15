namespace NetCourses.Models;

public class Response<T>
{
    public T? Data { get; set; }
    public int ResponseCode { get; set; }

    public string? Message { get; set; }
}