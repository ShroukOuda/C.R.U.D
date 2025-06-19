namespace CRUD.Services
{
    public interface IAuthorsService
    {
        Task<IEnumerable<Author>> GetAuthorsAsync();
        Task<Author> GetAuthorAsync(int id);
        Task<Author> CreateAuthorAsync(Author author);
        Author UpdateAuthorAsync(Author author);
        Author DeleteAuthorAsync(Author author);
        Task<bool> ExistingAuthor(string email);
        Task<bool> HasBooks(int authorId);
        Task<bool> IsValidAuthor(int authorId);
    }
}
