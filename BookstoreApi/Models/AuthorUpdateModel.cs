using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.Models
{
    /// <summary>
    /// Represents an author with optional image.
    /// </summary>
    public class AuthorUpdateModel
    {
        /// <summary>
        /// Gets  the author id.
        /// </summary>
        public int author_id { get; set; }
        /// <summary>
        /// Gets or sets the name of the author.
        /// </summary>

        public string? author_name { get; set; }

        /// <summary>
        /// Gets or sets the biography of the author.
        /// </summary>
        public string? biography { get; set; }
    }
}
