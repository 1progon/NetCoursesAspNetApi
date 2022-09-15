using NetCourses.Enums.Courses;

namespace NetCourses.Models.Courses;

public class CourseVideos : BaseModel
{
    public string VideoLink { get; set; } = null!;
    public CourseVideoSource VideoSource { get; set; }
    public int Order { get; set; }

    public string? Image { get; set; }

    public Course Course { get; set; } = null!;
    public long CourseId { get; set; }
}