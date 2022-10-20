using System.Text.Json.Serialization;

namespace NetCourses.Dto.Jobs;

public class PostJobDto
{
    [JsonPropertyName("title")] public string Title { get; set; } = null!;
    [JsonPropertyName("levelExpertise")] public string LevelExpertise { get; set; } = null!;
    [JsonPropertyName("description")] public string Description { get; set; } = null!;
    [JsonPropertyName("location")] public string Location { get; set; } = null!;
    [JsonPropertyName("urlToJob")] public string UrlToJob { get; set; } = null!;
    [JsonPropertyName("tags")] public string? Tags { get; set; }
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = null!;
}