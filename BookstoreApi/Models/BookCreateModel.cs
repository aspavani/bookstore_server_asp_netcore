// Models/BookCreateModel.cs
namespace BookstoreApi.Models
{
    public class BookCreateModel
    {
        public string? title { get; set; }
        public decimal price { get; set; }
        public DateTime publication_date { get; set; }
        public int author_id { get; set; }
        public int genre_id { get; set; }

    }
}