using NetCourses.Enums;
using NetCourses.Enums.Courses;

namespace NetCourses.Models.Courses;

public class Course : BaseModel
{
    public override long Id { get; set; }
    public string? Link { get; set; }

    public string? VideoLink { get; set; }
    public CourseVideoSource? VideoSource { get; set; }

    public DateOnly PostedByAuthor { get; set; }
    public DateOnly? UpdatedByAuthor { get; set; }

    public string Description { get; set; } = null!;
    public string? Article { get; set; }
    public string? Image { get; set; }

    public CourseVideoType VideoType { get; set; }

    public Status Status { get; set; }

    public Language? Language { get; set; }
    public int? LanguageId { get; set; }

    public IList<CourseVideos> CourseVideos { get; set; } = new List<CourseVideos>();
}