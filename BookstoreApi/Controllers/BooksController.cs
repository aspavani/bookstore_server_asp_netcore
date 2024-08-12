using BookstoreApi.DBContext;
using BookstoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BookstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BooksController : ControllerBase
    {
        private readonly BookstoreContext bscontext;

        public BooksController(BookstoreContext context)
        {
            bscontext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return Ok(await bscontext.Books.Include(b => b.Author).Include(b => b.Genre).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await bscontext.Books.Include(b => b.Author).Include(b => b.Genre).FirstOrDefaultAsync(b => b.book_id == id);

            if (book == null)
            {
                // Return 404 if the book is not found
                return NotFound();
            }

            return Ok(book);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await bscontext.Books.FindAsync(id);
                if (book == null)
                {
                    // Return 404 if the book is not found
                    return NotFound(new { error = "Book not found" });
                }

                bscontext.Books.Remove(book);
                await bscontext.SaveChangesAsync();

                // Return 204 for a successful deletion with no content
                return NoContent();
            }
            catch (System.Exception ex)
            {
                // Return 500 for an internal server error with the exception message
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            if (book == null)
            {
                return BadRequest("Book is null.");
            }

            // Convert model to entity
            var bookEntity = new BookstoreApi.Entities.Book
            {
                title = book.title,
                price = book.price,
                publication_date = book.publication_date,
                imageUrl = book.imageUrl,
                author_id = book.author_id,
                genre_id = book.genre_id
            };

            bscontext.Books.Add(bookEntity);
            await bscontext.SaveChangesAsync();

            // Optionally: Return the created book with its ID
            book.book_id = bookEntity.book_id;

            return CreatedAtAction(nameof(GetBooks), new { id = book.book_id }, book);
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
        // {
        // if (book == null)
        // {
        //     return BadRequest("Book data is null.");
        // }

        // Check if the ID from the route matches the ID in the book data
        // if (id != book.book_id)
        // {
        //     return BadRequest("ID in the URL does not match the ID in the book data.");
        // }

        // Find the existing book
        // var existingBook = await bscontext.Books.FindAsync(id);
        // if (existingBook == null)
        // {
        //     return NotFound();
        // }

        // Update the existing book's properties with the new values
        // existingBook.title = book.title;
        // existingBook.price = book.price;
        // existingBook.publication_date = book.publication_date;
        // existingBook.imageUrl = book.imageUrl;
        // existingBook.author_id = book.author_id;
        // existingBook.genre_id = book.genre_id;


        // // Update fields only if they are provided in the request
        // if (!string.IsNullOrEmpty(book.title))
        // {
        //     existingBook.title = book.title;
        // }

        // if (book.price > 0)
        // {
        //     existingBook.price = book.price;
        // }

        // if (book.publication_date != default)
        // {
        //     existingBook.publication_date = book.publication_date;
        // }

        // if (!string.IsNullOrEmpty(book.imageUrl))
        // {
        //     existingBook.imageUrl = book.imageUrl;
        // }

        // if (book.author_id > 0)
        // {
        //     existingBook.author_id = book.author_id;
        // }

        //     if (book.genre_id > 0)
        //     {
        //         existingBook.genre_id = book.genre_id;
        //     }

        //     try
        //     {
        //         // Save changes to the database
        //         await bscontext.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         // Handle concurrency issues
        //         if (!BookExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        private bool BookExists(int id)
        {
            return bscontext.Books.Any(e => e.book_id == id);
        }

        // ***************************


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateModel updateModel)
        {
            if (updateModel == null)
            {
                return BadRequest("Book data is null.");
            }

            // Check if the ID from the route matches the ID in the update data
            if (id != updateModel.book_id)
            {
                return BadRequest("ID in the URL does not match the ID in the update data.");
            }

            // Find the existing book
            var existingBook = await bscontext.Books.FindAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            // Update fields only if they are provided in the request
            if (!string.IsNullOrEmpty(updateModel.title))
            {
                existingBook.title = updateModel.title;
            }

            if (updateModel.price.HasValue)
            {
                existingBook.price = updateModel.price.Value;
            }

            if (updateModel.publication_date.HasValue)
            {
                existingBook.publication_date = updateModel.publication_date.Value;
            }

            if (!string.IsNullOrEmpty(updateModel.imageUrl))
            {
                existingBook.imageUrl = updateModel.imageUrl;
            }

            if (updateModel.author_id.HasValue)
            {
                existingBook.author_id = updateModel.author_id.Value;
            }

            if (updateModel.genre_id.HasValue)
            {
                existingBook.genre_id = updateModel.genre_id.Value;
            }

            try
            {
                // Save changes to the database
                await bscontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



    }
}