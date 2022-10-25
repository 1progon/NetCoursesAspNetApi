using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCourses.Data;
using NetCourses.Dto;
using NetCourses.Dto.Courses;
using NetCourses.Enums;
using NetCourses.Models;
using NetCourses.Models.Courses;

namespace NetCourses.Controllers.Courses;

[Route("api/v1/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public CoursesController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // GET: api/v1/Courses
    [HttpGet]
    public async Task<ActionResult<Response<GetItemsDto<Course>>>> GetCourses(
        [FromQuery] int limit = 20, [FromQuery] int offset = 0)
    {
        if (!_context.Courses.Any()) return NotFound();

        return new Response<GetItemsDto<Course>>
        {
            Data = new GetItemsDto<Course>
            {
                Items = await _context.Courses
                    .OrderByDescending(c => c.PostedByAuthor)
                    .Include(c => c.Language)
                    .Skip(offset)
                    .Take(limit)
                    .Where(c => c.Status == Status.Active)
                    .ToListAsync(),
                Limit = limit,
                Offset = offset,
                Count = await _context.Courses.CountAsync()
            },
            ResponseCode = 200
        };
    }

    // GET: api/v1/Courses/5
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Response<Course>>> GetCourse(long id)
    {
        if (!_context.Courses.Any()) return NotFound();

        var course = await _context.Courses
            .Where(c => c.Status == Status.Active)
            .Include(c
                => c.CourseVideos.OrderBy(v => v.Order))
            .Include(c => c.Language)
            .SingleOrDefaultAsync(c => c.Id == id);

        if (course == null) return NotFound();


        return new Response<Course>()
        {
            Data = course,
            ResponseCode = 200
        };
    }

    // PUT: api/Courses/5
    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:long}")]
    [Authorize]
    public async Task<IActionResult> PutCourse(long id, Course course)
    {
        if (id != course.Id) return BadRequest();

        _context.Entry(course).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CourseExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Courses
    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Response<Course>>> PostCourse(
        [FromForm] string courseDto,
        [FromForm] IFormFile file)
    {
        var courseForm = JsonSerializer.Deserialize<PostCourseDto>(courseDto);

        if (courseForm == null) return UnprocessableEntity();

        var course = new Course
        {
            Name = courseForm.Name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Link = courseForm.Link,
            VideoLink = courseForm.VideoLink,
            VideoSource = courseForm.VideoSource,
            PostedByAuthor = DateOnly.Parse(courseForm.PostedByAuthor.ToShortDateString()),
            Description = courseForm.Description,
            Article = courseForm.Article,
            Image = await SaveFile(file),
            VideoType = courseForm.VideoType,
            Status = Status.Moderation,
            LanguageId = courseForm.LanguageId,
        };


        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return new Response<Course>
        {
            Data = course,
            ResponseCode = 200,
        };
    }

    // DELETE: api/Courses/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteCourse(long id)
    {
        if (!_context.Courses.Any()) return NotFound();

        var course = await _context.Courses.FindAsync(id);
        if (course == null) return NotFound();

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CourseExists(long id)
    {
        return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    //todo move method to separate for diff controllers
    private async Task<string> SaveFile(IFormFile imageFile)
    {
        var newFileName = Guid.NewGuid();
        var fileExtension = Path.GetExtension(imageFile.FileName);
        var path = $"/images/Files/Courses/{newFileName}{fileExtension}";
        try
        {
            await using var fileStream = new FileStream(
                _environment.ContentRootPath + path, FileMode.CreateNew);

            await imageFile.CopyToAsync(fileStream);
        }
        catch (IOException e)
        {
            // await SaveFile(imageFile);

            Console.WriteLine(e.Message);
        }

        return path;
    }
}