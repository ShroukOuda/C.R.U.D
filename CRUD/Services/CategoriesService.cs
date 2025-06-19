using CRUD.Data;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext _context;
        public CategoriesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            
            return category;
        }

        public Category DeleteCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChangesAsync();

            return category;
        }

        public async Task<bool> ExistingCategory(string name)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.Books)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Books)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> HasBooks(int categoryId)
        {
            return await _context.Books.AnyAsync(b => b.CategoryId == categoryId);
        }

        public Category UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChangesAsync();
            return category;
        }

        public Task<bool> IsValidCategory(int categoryId)
        {
            return  _context.Categories.AnyAsync(c => c.Id == categoryId);  
        }
    }
}
