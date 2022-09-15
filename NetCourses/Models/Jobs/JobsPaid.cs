using System.ComponentModel.DataAnnotations;

namespace NetCourses.Models.Jobs;

public class JobsPaid
{
    [Key] public long Id { get; set; }
    public string? BackgroundColor { get; set; }
    public string? ColorBorder { get; set; }
    public bool Homepage { get; set; }
    public bool Topped { get; set; }

    public Job Job { get; set; } = null!;
    public long JobId { get; set; }
}