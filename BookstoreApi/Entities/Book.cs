using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreApi.Entities
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int book_id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar")]
        public string? title { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal price { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime publication_date { get; set; }

        public string? imageUrl { get; set; } = null;

        [ForeignKey("author_id")]
        [Required]
        public Author? Author { get; set; }
        public int author_id { get; set; }

        [Required]
        [ForeignKey("genre_id")]
        public Genre? Genre { get; set; }
        public int genre_id { get; set; }

    }
}