using BookstoreApi.DBContext;
using BookstoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BookstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthorsController : ControllerBase
    {
        private readonly BookstoreContext bscontext;

        public AuthorsController(BookstoreContext context)
        {
            bscontext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return Ok(await bscontext.Authors.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetAuthor(int id)
        {
            var author = await bscontext.Authors.FirstOrDefaultAsync(a => a.author_id == id);

            if (author == null)
            {
                // Return 404 if the book is not found
                return NotFound();
            }

            return Ok(author);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var author = await bscontext.Authors.FindAsync(id);
                if (author == null)
                {
                    // Return 404 if the book is not found
                    return NotFound(new { error = "Author not found" });
                }

                bscontext.Authors.Remove(author);
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
        public async Task<ActionResult<Author>> CreateAuthor(Author author)
        {
            if (author == null)
            {
                return BadRequest("Author is null.");
            }

            // Convert model to entity
            var AuthorEntity = new BookstoreApi.Entities.Author
            {
                author_name = author.author_name,
                biography = author.biography,
                imageUrl = author.imageUrl

            };

            bscontext.Authors.Add(AuthorEntity);
            await bscontext.SaveChangesAsync();

            // Optionally: Return the created book with its ID
            author.author_id = AuthorEntity.author_id;

            return CreatedAtAction(nameof(GetAuthors), new { id = author.author_id }, author);
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

        private bool AuthorExists(int id)
        {
            return bscontext.Authors.Any(e => e.author_id == id);
        }

        // ***************************


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author updateModel)
        {
            if (updateModel == null)
            {
                return BadRequest("Author data is null.");
            }

            // Check if the ID from the route matches the ID in the update data
            if (id != updateModel.author_id)
            {
                return BadRequest("ID in the URL does not match the ID in the update data.");
            }

            // Find the existing book
            var existingAuthor = await bscontext.Authors.FindAsync(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            // Update fields only if they are provided in the request
            if (!string.IsNullOrEmpty(updateModel.biography))
            {
                existingAuthor.biography = updateModel.biography;
            }

            if (!string.IsNullOrEmpty(updateModel.author_name))
            {
                existingAuthor.author_name = updateModel.author_name;
            }

            if (!string.IsNullOrEmpty(updateModel.imageUrl))
            {
                existingAuthor.imageUrl = updateModel.imageUrl;
            }





            try
            {
                // Save changes to the database
                await bscontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues
                if (!AuthorExists(id))
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