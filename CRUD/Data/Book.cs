using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Data
{
    public class Book
    {
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Range(1000, 9999, ErrorMessage = "Year must be a valid 4-digit year")]
        public int Year { get; set; }

        [Range(0.01, 99999.99, ErrorMessage = "Price must be between $0.01 and $99,999.99")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [MaxLength(3 * 1024 * 1024)]
        public byte[]? Cover { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }

        // Navigation Properties
        public virtual Author? Author { get; set; }
        public virtual Category? Category { get; set; }
    }
}