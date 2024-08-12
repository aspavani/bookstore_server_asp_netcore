using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.Models
{
    /// <summary>
    /// Genre Details
    /// </summary>
    public class Genre
    {
        /// <summary>
        /// Genre Id
        /// </summary>
        public int genre_id { get; set; }
        /// <summary>
        /// Genre Name
        /// </summary>
        public string? genre_name { get; set; }
    }
}