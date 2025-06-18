using CRUD.Data;
using CRUD.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Select(b => new BookDetailsDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    Price = b.Price,
                    AuthorId = b.AuthorId,
                    CategoryId = b.CategoryId,
                    Cover = b.Cover,
                    AuthorFullName = b.Author.FullName,
                    CategoryName = b.Category.Name
                })
                .ToListAsync();

            return Ok(books);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .SingleOrDefaultAsync(b => b.Id == id);
            
            if (book == null)
                return NotFound();
            
            var dto = new BookDetailsDto
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                Price = book.Price,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                Cover = book.Cover,
                AuthorFullName = book.Author.FullName,
                CategoryName = book.Category.Name
            };
            return Ok(dto);
        }
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetBooksByCategory(int categoryId)
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.CategoryId == categoryId)
                .Select(b => new BookDetailsDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    Price = b.Price,
                    AuthorId = b.AuthorId,
                    CategoryId = b.CategoryId,
                    Cover = b.Cover,
                    AuthorFullName = b.Author.FullName,
                    CategoryName = b.Category.Name
                })
                .ToListAsync();
            
            if (books == null || !books.Any())
                return NotFound($"No books found for category ID: {categoryId}");
            
            return Ok(books);
        }
        [HttpGet("author/{authorId}")]
        public async Task<IActionResult> GetBooksByAuthor(int authorId)
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.AuthorId == authorId)
                .Select(Author => new BookDetailsDto
                {
                    Id = Author.Id,
                    Title = Author.Title,
                    Year = Author.Year,
                    Price = Author.Price,
                    AuthorId = Author.AuthorId,
                    CategoryId = Author.CategoryId,
                    Cover = Author.Cover,
                    AuthorFullName = Author.Author.FullName,
                    CategoryName = Author.Category.Name
                })
                .ToListAsync();
            
            if (books == null || !books.Any())
                return NotFound($"No books found for author ID: {authorId}");
            
            return Ok(books);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromForm] BookDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Cover == null)
                return BadRequest("Cover image is required!");

            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Cover.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");
            
            using var dataStream = new MemoryStream();
            await dto.Cover.CopyToAsync(dataStream);

            var book = new Book
            {
                Title = dto.Title,
                Year = dto.Year,
                Price = dto.Price,
                AuthorId = dto.AuthorId,
                CategoryId = dto.CategoryId,
                Cover = dataStream.ToArray()
            };
           
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound($"No Book Was Found With ID: {id}");

            if (dto.Cover == null)
                return BadRequest("Cover image is required!");

            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Cover.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");
           
            using var dataStream = new MemoryStream();
            await dto.Cover.CopyToAsync(dataStream);


            book.Title = dto.Title;
            book.Year = dto.Year;
            book.Price = dto.Price;
            book.AuthorId = dto.AuthorId;
            book.CategoryId = dto.CategoryId;
            book.Cover = dataStream.ToArray();

            
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(book);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound($"No Book Was Found With ID: {id}");
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok($"Book with ID: {id} was deleted successfully.");
        }
    }
}
