namespace CRUD.Dtos
{
    public class BookDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public byte[] Cover { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string AuthorFullName { get; set; } = string.Empty;
    }
}
