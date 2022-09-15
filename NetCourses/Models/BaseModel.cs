using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NetCourses.Models;

[Index(nameof(Slug), IsUnique = true)]
public class BaseModel
{
    [Key] public virtual long Id { get; set; }

    [Required] public string Name { get; set; } = null!;
    [Required] public string Slug { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}