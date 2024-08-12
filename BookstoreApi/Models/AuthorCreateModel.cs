using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.Models
{
    /// <summary>
    /// Represents an author with optional image.
    /// </summary>
    public class AuthorCreateModel
    {

        /// <summary>
        /// Gets or sets the name of the author.
        /// </summary>
        [Required(ErrorMessage = "Author name is required.")]
        public string author_name { get; set; }

        /// <summary>
        /// Gets or sets the biography of the author.
        /// </summary>
        public string? biography { get; set; }
    }
}
