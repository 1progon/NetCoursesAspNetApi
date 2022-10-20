using System.ComponentModel.DataAnnotations;

namespace NetCourses.Models;

public class BaseModel
{
    [Key] public virtual long Id { get; set; }

    [Required] public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}