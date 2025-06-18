using CRUD.Data;
using CRUD.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .Include(c => c.Books)
                .ToListAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Books)  
                .SingleOrDefaultAsync(c => c.Id == id);
            if (category == null)
                return NotFound($"No Category Was Found With ID: {id}");
            
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm]CategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == dto.Name.ToLower());

            if (existingCategory != null)
                return BadRequest($"A Category with the name {dto.Name} already exists.");
            
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };
        
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id,[FromForm] CategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound($"No Category Was Found With ID: {id}");
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == dto.Name.ToLower() && c.Id != id);
            if (existingCategory != null)
                return BadRequest($"A Category with the name {dto.Name} already exists.");

            category.Name = dto.Name;
            category.Description = dto.Description;

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return Ok(category);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound($"No Category Was Found With ID: {id}");
            
            var hasBooks = await _context.Books.AnyAsync(b => b.CategoryId == id);
            if (hasBooks)
                return BadRequest("Cannot delete a category that has associated books.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok($"Category with ID: {id} was deleted successfully.");
        }
    }
}
