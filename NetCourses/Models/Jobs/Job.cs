using NetCourses.Enums;
using NetCourses.Models.Companies;

namespace NetCourses.Models.Jobs;

public class Job : BaseModel
{
    public override long Id { get; set; }
    public string Level { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Article { get; set; }
    public string? Image { get; set; }
    public string? Location { get; set; }
    public string Url { get; set; } = null!;

    public string[]? Tags { get; set; }

    public Company Company { get; set; } = null!;
    public long CompanyId { get; set; }

    public JobsPaid? Paid { get; set; }

    public Status Status { get; set; }
}