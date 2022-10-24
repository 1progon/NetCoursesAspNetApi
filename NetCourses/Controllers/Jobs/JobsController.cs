using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCourses.Data;
using NetCourses.Dto;
using NetCourses.Dto.Jobs;
using NetCourses.Enums.Jobs;
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
            .Where(j => j.Status == JobStatus.Active)
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
            .Where(j => j.Status == JobStatus.Active ||
                        j.Status == JobStatus.Moderation ||
                        j.Status == JobStatus.ArchivedByUser ||
                        j.Status == JobStatus.ArchivedByAdmin)
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

    // POST: api/v1/Jobs
    [HttpPost]
    public async Task<ActionResult<Response<Job>>> PostJob(
        [FromForm] string form, [FromForm] IFormFile file)
    {
        var jobForm = JsonSerializer.Deserialize<PostJobDto>(form);

        if (jobForm == null) return UnprocessableEntity();

        // get company from db or create
        var company = await _context.Companies
            .SingleOrDefaultAsync(c => c.Name == jobForm.CompanyName);
        if (company == null)
        {
            company = new Company
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = jobForm.CompanyName,
                LogoImagePath = await SaveFile(file)
            };
            _context.Companies.Add(company);
        }


        var job = new Job
        {
            Name = jobForm.Title,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Level = jobForm.LevelExpertise,
            Description = jobForm.Description,
            Location = jobForm.Location,
            Url = jobForm.UrlToJob,
            Tags = jobForm.Tags?.Split(","),
            Company = company,
            Status = JobStatus.Moderation,
        };

        _context.Jobs.Add(job);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Problem("not saved to db: " + e.InnerException);
        }


        return new Response<Job>()
        {
            Data = job,
            ResponseCode = 200,
        };
    }

    // DELETE: api/Jobs/5
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteJob(long id)
    {
        if (!_context.Jobs.Any()) return NotFound();

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

    private async Task<string> SaveFile(IFormFile imageFile)
    {
        var newFileName = Guid.NewGuid();
        var fileExtension = Path.GetExtension(imageFile.FileName);
        var path = $"/Files/Companies/{newFileName}{fileExtension}";
        try
        {
            await using var fileStream = new FileStream(
                _environment.WebRootPath + path, FileMode.CreateNew);

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