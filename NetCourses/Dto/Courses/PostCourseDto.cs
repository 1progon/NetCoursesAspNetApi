using System.Text.Json.Serialization;
using NetCourses.Enums.Courses;

namespace NetCourses.Dto.Courses;

public class PostCourseDto
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("link")] public string? Link { get; set; }
    [JsonPropertyName("videoLink")] public string? VideoLink { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; } = null!;
    [JsonPropertyName("article")] public string? Article { get; set; }
    [JsonPropertyName("videoType")] public CourseVideoType VideoType { get; set; }
    [JsonPropertyName("videoSource")] public CourseVideoSource VideoSource { get; set; }
    [JsonPropertyName("postedByAuthor")] public DateTime PostedByAuthor { get; set; }
    [JsonPropertyName("languageId")] public int LanguageId { get; set; }
}