namespace NetCourses.Models.Companies;

public class Company : BaseModel
{
    public string? Url { get; set; }
    public int? Foundation { get; set; }
    public string? Country { get; set; }
}