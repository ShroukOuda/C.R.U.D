using CRUD.Data;
using CRUD.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class BooksService : IBooksService
    {
        private readonly ApplicationDbContext _context;
        public BooksService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Book> CreateBookAsync(Book book)
        {
            await _context.AddAsync(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public Book DeleteBookAsync(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChangesAsync();

            return book;
        }

        public async Task<Book> GetBookAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .SingleOrDefaultAsync(b => b.Id == id);

        }

        public async Task<IEnumerable<Book>> GetBooksAsync(int categoryId = 0, int authorId = 0)
        {
            var books = await _context.Books
                .Where(b => b.CategoryId == categoryId || categoryId == 0)
                .Where(b => b.AuthorId == authorId || authorId == 0)
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();
            
            return books;
        }

        public Book UpdateBookAsync(Book book)
        {
            _context.Update(book);
            _context.SaveChangesAsync();
               
            return book;
        }
    }
}
