namespace CRUD.Services
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(int id);
        Task<Category> CreateCategoryAsync(Category category);
        Category UpdateCategoryAsync(Category category);
        Category DeleteCategoryAsync(Category category);
        Task<bool> ExistingCategory(string name);
        Task<bool> HasBooks(int categoryId);
        Task<bool> IsValidCategory(int categoryId);

    }
}
