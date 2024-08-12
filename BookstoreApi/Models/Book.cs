using BookstoreApi.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.Models
{
    /// <summary>
    /// Book Details
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Book Id
        /// </summary>

        public int book_id { get; set; }
        /// <summary>
        /// Book Title
        /// </summary>
        public string? title { get; set; }
        /// <summary>
        /// Book Price
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// Book's Publication Date
        /// </summary>
        public DateTime publication_date { get; set; }
        /// <summary>
        /// Book's Image
        /// </summary>
        public string? imageUrl { get; set; } = "";
        /// <summary>
        /// Author Details
        /// </summary>
        public Author? Author { get; set; }
        /// <summary>
        /// Author Id
        /// </summary>
        public int author_id { get; set; }
        /// <summary>
        /// Genre Details
        /// </summary>
        public Genre? Genre { get; set; }
        /// <summary>
        /// Genre Id
        /// </summary>
        public int genre_id { get; set; }
        /// <summary>
        /// OfferId
        /// </summary>

    }
}