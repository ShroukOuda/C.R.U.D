using CRUD.Data;
using CRUD.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _context.Authors.ToListAsync();
            return Ok(authors);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound($"No Author Was Found With ID: {id}");
            }
            return Ok(author);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingAuthor = await _context.Authors
                .FirstOrDefaultAsync(a => a.Email == dto.Email);
            if (existingAuthor != null)
                return BadRequest($"An Author with the email {dto.Email} already exists.");
            
            var author = new Author
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Nationality = dto.Nationality,
                BirthDate = dto.BirthDate
            };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                return NotFound($"No Author Was Found With ID: {id}");
            
            var existingAuthor = await _context.Authors
                .FirstOrDefaultAsync(a => a.Email == dto.Email && a.Id != id);
            if (existingAuthor != null)
                return BadRequest($"An Author with the email {dto.Email} already exists.");

            author.FirstName = dto.FirstName;
            author.LastName = dto.LastName;
            author.Email = dto.Email;
            author.Nationality = dto.Nationality;
            author.BirthDate = dto.BirthDate;

            _context.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(author);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                return NotFound($"No Author Was Found With ID: {id}");
            
            var hasBooks = await _context.Books.AnyAsync(b => b.AuthorId == id);
            if (hasBooks)
                return BadRequest("Cannot delete an author who has books associated with them. Please delete the books first.");
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            
            return Ok($"Author with ID: {id} was deleted successfully.");
        }
    }
}
