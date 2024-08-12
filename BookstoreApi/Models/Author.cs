using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.Models
{
    /// <summary>
    /// Author's Details
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Author Id
        /// </summary>
        public int author_id { get; set; }
        /// <summary>
        /// Name of author
        /// </summary>
        public string? author_name { get; set; }
        /// <summary>
        /// author's biography
        /// </summary>
        public string? biography { get; set; }
        /// <summary>
        /// Image of Author
        /// </summary>
        public string? imageUrl { get; set; } = null;

    }
}