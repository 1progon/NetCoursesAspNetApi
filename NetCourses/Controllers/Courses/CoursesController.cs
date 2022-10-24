using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCourses.Data;
using NetCourses.Dto;
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
                Items = await _context.Courses.OrderBy(c => c.Id)
                    .Skip(offset)
                    .Take(limit)
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
            .Include(c
                => c.CourseVideos.OrderBy(v => v.Order))
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
    public async Task<ActionResult<Course>> PostCourse(Course course)
    {
        if (!_context.Courses.Any()) return Problem("Entity set 'AppDbContext.Courses'  is null.");

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCourse", new {id = course.Id}, course);
    }

    // DELETE: api/Courses/5
    [HttpDelete("{id}")]
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
}