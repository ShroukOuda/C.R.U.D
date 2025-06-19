namespace CRUD.Data.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 characters")]
        public string? Description { get; set; }

        //NotMapped Properties
        [NotMapped]
        public int BookCount => Books?.Count ?? 0;

        // Navigation Properties
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}