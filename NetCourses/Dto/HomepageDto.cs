using NetCourses.Models.Courses;
using NetCourses.Models.Jobs;

namespace NetCourses.Dto;

public class HomepageDto
{
    public IEnumerable<Job> Jobs { get; set; } = null!;
    public IEnumerable<Course> Courses { get; set; } = null!;
}