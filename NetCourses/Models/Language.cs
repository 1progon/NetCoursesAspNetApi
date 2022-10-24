using System.ComponentModel.DataAnnotations;

namespace NetCourses.Models;

public class Language
{
    [Key] public int Id { get; set; }
    [Required] public string Name { get; set; } = null!;
}