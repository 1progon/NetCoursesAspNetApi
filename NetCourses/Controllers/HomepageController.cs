using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCourses.Data;
using NetCourses.Dto;
using NetCourses.Models;

namespace NetCourses.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class HomepageController : ControllerBase
{
    private readonly AppDbContext _context;

    public HomepageController(AppDbContext context) => _context = context;

    // GET: api/v1/Homepage
    [HttpGet]
    public async Task<ActionResult<Response<HomepageDto>>> Homepage()
    {
        var r = new Response<HomepageDto>
        {
            Data = new HomepageDto
            {
                // get homepage jobs
                Jobs = await _context.Jobs
                    .Include(j => j.Paid)
                    .OrderBy(j => j.Id)
                    .Skip(0)
                    .Take(5)
                    .ToListAsync(),

                // get homepage courses
                Courses = await _context.Courses
                    .OrderBy(c => c.Id)
                    .Skip(0)
                    .Take(5)
                    .ToListAsync()
            },
            ResponseCode = 200
        };


        return r;
    }
}