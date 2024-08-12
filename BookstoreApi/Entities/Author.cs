using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreApi.Entities
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int author_id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar")]
        public string? author_name { get; set; }

        [Required]
        [MaxLength(200)]
        [Column(TypeName = "varchar")]
        public string? biography { get; set; }

        public string? imageUrl { get; set; } = null;

    }
}