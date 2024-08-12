using BookstoreApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApi.DBContext
{
    public class BookstoreContext : DbContext
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Authors
            modelBuilder.Entity<Author>().HasData(
                new Author { author_id = 1, author_name = "J.K. Rowling", biography = "British author, best known for the Harry Potter series.", imageUrl = " " },
                new Author { author_id = 2, author_name = "J.R.R. Tolkien", biography = "English writer and professor, known for The Hobbit and The Lord of the Rings.", imageUrl = " " }
            );

            // Seed Genres
            modelBuilder.Entity<Genre>().HasData(
                new Genre { genre_id = 1, genre_name = "Fantasy" },
                new Genre { genre_id = 2, genre_name = "Science Fiction" }
            );

            // Seed Books
            modelBuilder.Entity<Book>().HasData(
            new Book { book_id = 1, title = "Harry Potter and the Sorcerer's Stone", author_id = 1, genre_id = 1, price = 19.99m, publication_date = new DateTime(1997, 6, 26), imageUrl = " " },
            new Book { book_id = 2, title = "The Hobbit", author_id = 2, genre_id = 1, price = 14.99m, publication_date = new DateTime(1937, 9, 21), imageUrl = " " }
            //new Book { book_id = 3, title = "Dune", AuthorId = 2, GenreId = 2, Price = 25.00m, PublicationDate = new DateTime(1965, 8, 1) }
            );
        }
    }
}