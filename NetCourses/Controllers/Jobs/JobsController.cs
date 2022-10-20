using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCourses.Data;
using NetCourses.Dto;
using NetCourses.Models;
using NetCourses.Models.Companies;
using NetCourses.Models.Jobs;

namespace NetCourses.Controllers.Jobs;

[Route("api/v1/[controller]")]
[ApiController]
public class JobsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public JobsController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // GET: api/v1/Jobs
    [HttpGet]
    public async Task<ActionResult<Response<GetItemsDto<Job>>>> GetJobs(
        [FromQuery] int limit = 20, [FromQuery] int offset = 0)
    {
        if (!_context.Jobs.Any()) return NotFound();

        var jobsCount = await _context.Jobs.CountAsync();

        var jobs = await _context.Jobs
            .Include(j => j.Paid)
            .Include(j => j.Company)
            .OrderByDescending(j => j.UpdatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        return new Response<GetItemsDto<Job>>
        {
            Data = new GetItemsDto<Job>
            {
                Limit = limit,
                Offset = offset,
                Items = jobs,
                Count = jobsCount
            },
            ResponseCode = 200
        };
    }

    // GET: api/v1/Jobs/5
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Response<Job>>> GetJob(long id)
    {
        if (!_context.Jobs.Any()) return NotFound();

        var job = await _context.Jobs
            .Include(j => j.Company)
            .SingleOrDefaultAsync(j => j.Id == id);

        if (job == null) return NotFound();

        return new Response<Job>
        {
            Data = job,
            ResponseCode = 200
        };
    }

    // PUT: api/Jobs/5
    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:long}")]
    public async Task<IActionResult> PutJob(long id, Job job)
    {
        if (id != job.Id) return BadRequest();

        _context.Entry(job).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!JobExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Jobs
    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Job>> PostJob(Job job)
    {
        if (!_context.Jobs.Any())
            return Problem("'Jobs'  is null.");

        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetJob", new {id = job.Id}, job);
    }

    // DELETE: api/Jobs/5
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteJob(long id)
    {
        if (_context.Jobs == null) return NotFound();

        var job = await _context.Jobs.FindAsync(id);
        if (job == null) return NotFound();

        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool JobExists(long id)
    {
        return (_context.Jobs?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}