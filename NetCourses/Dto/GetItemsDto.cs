namespace NetCourses.Dto;

public class GetItemsDto<T>
{
    public int Limit { get; set; }
    public int Offset { get; set; }
    public int Count { get; set; }
    public IEnumerable<T> Items { get; set; } = null!;
}