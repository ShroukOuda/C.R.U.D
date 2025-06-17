using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Data
{
    public class Author
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
        public string LastName { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(254)]
        public string Email { get; set; } = string.Empty;
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nationality must be between 2 and 100 characters")]
        public string Nationality { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }


        // NotMapped Properties
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();
        [NotMapped]
        public int Age => DateTime.Now.Year - BirthDate.Year -
                         (DateTime.Now.DayOfYear < BirthDate.DayOfYear ? 1 : 0);

        // Navigation Properties
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}