using CRUD.Data;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class AuthorsService : IAuthorsService
    {
        private readonly ApplicationDbContext _context;

        public AuthorsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await _context.Authors
                .Include(a => a.Books)
                .ToListAsync();
        }

        public async Task<Author> GetAuthorAsync(int id)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Author> CreateAuthorAsync(Author author)
        {
            await _context.AddAsync(author);
            _context.SaveChanges();
            return author;
        }

        public Author UpdateAuthorAsync(Author author)
        {
            _context.Update(author);
            _context.SaveChanges();
            return author;
        }

        public Author DeleteAuthorAsync(Author author)
        {
            _context.Remove(author);
            _context.SaveChanges();
            return author;
        }

        public Task<bool> ExistingAuthor(string email)
        {
            return _context.Authors.AnyAsync(a => a.Email == email);
        }
        public async Task<bool> HasBooks(int authorId)
        {
            return await _context.Books.AnyAsync(b => b.AuthorId == authorId);
        }

        public Task<bool> IsValidAuthor(int authorId)
        {
            return _context.Authors.AnyAsync(a => a.Id == authorId);
        }
    }
}
