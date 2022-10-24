using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCourses.Data;
using NetCourses.Models;

namespace NetCourses.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LanguagesController(AppDbContext context) => _context = context;

        // GET: api/v1/Languages
        [HttpGet]
        public async Task<ActionResult<Response<IList<Language>>>> GetLanguages()
        {
            if (!await _context.Languages.AnyAsync()) return NotFound();

            return new Response<IList<Language>>
            {
                ResponseCode = 200,
                Data = await _context.Languages.ToListAsync()
            };
        }

        // GET: api/v1/Languages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Language>> GetLanguage(int id)
        {
            if (_context.Languages == null)
            {
                return NotFound();
            }

            var language = await _context.Languages.FindAsync(id);

            if (language == null)
            {
                return NotFound();
            }

            return language;
        }

        // PUT: api/v1/Languages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutLanguage(int id, Language language)
        {
            if (id != language.Id)
            {
                return BadRequest();
            }

            _context.Entry(language).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/v1/Languages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Language>> PostLanguage(Language language)
        {
            if (_context.Languages == null)
            {
                return Problem("Entity set 'AppDbContext.Languages'  is null.");
            }

            _context.Languages.Add(language);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLanguage", new { id = language.Id }, language);
        }

        // DELETE: api/v1/Languages/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLanguage(int id)
        {
            if (_context.Languages == null)
            {
                return NotFound();
            }

            var language = await _context.Languages.FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }

            _context.Languages.Remove(language);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LanguageExists(int id)
        {
            return (_context.Languages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}