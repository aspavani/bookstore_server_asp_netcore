namespace BookstoreApi.Models
{
    public class BookUpdateModel
    {
        public int book_id { get; set; }
        public string? title { get; set; }
        public decimal? price { get; set; } // Nullable to represent optional
        public DateTime? publication_date { get; set; } // Nullable to represent optional

        public int? author_id { get; set; } // Nullable to represent optional
        public int? genre_id { get; set; } // Nullable to represent optional
    }
}
