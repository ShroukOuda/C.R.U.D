namespace CRUD.Services
{
    public interface IBooksService
    {
        Task<IEnumerable<Book>> GetBooksAsync(int categoryId = 0, int authorId = 0);
        Task<Book> GetBookAsync(int id);
        Task<Book> CreateBookAsync(Book book);
        Book UpdateBookAsync(Book book);
        Book DeleteBookAsync(Book book);
  
    }
}
