namespace CRUD.Dtos
{
    public class BookDto
    {
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public IFormFile Cover { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }
}
