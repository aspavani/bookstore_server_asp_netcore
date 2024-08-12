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

        // Inject the logger in addition to the context
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(BookstoreContext context, ILogger<AuthorsController> logger)
        {
            bscontext = context;
            _logger = logger;
        }


        /// <summary>
        /// Retrieves a list of all authors.
        /// </summary>
        /// <returns>Returns a list of authors.</returns>
        /// <response code="200">Returns a list of authors.</response>
        /// <response code="500">An error occurred while retrieving the authors.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            try
            {
                var authors = await bscontext.Authors.ToListAsync();

                // Return 200 OK with the list of authors
                return Ok(authors);
            }
            catch (System.Exception ex)
            {
                // Log the error and return 500 for internal server error
                _logger.LogError(ex, "An error occurred while retrieving the authors.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }


        /// <summary>
        /// Retrieves a specific author by ID.
        /// </summary>
        /// <param name="id">The ID of the author to retrieve.</param>
        /// <returns>Returns the author with the specified ID.</returns>
        /// <response code="200">Returns the author with the specified ID.</response>
        /// <response code="404">If no author is found with the specified ID.</response>
        /// <response code="500">An error occurred while retrieving the author.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetAuthor(int id)
        {
            try
            {
                var author = await bscontext.Authors.FirstOrDefaultAsync(a => a.author_id == id);

                if (author == null)
                {
                    // Return 404 Not Found if the author is not found
                    return NotFound();
                }

                // Return 200 OK with the author data
                return Ok(author);
            }
            catch (System.Exception ex)
            {
                // Log the error and return 500 for internal server error
                _logger.LogError(ex, "An error occurred while retrieving the author with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }


        /// <summary>
        /// Deletes an author by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the author to be deleted.</param>
        /// <returns>Returns a NoContent response if the author was successfully deleted.</returns>
        /// <response code="204">The author was successfully deleted.</response>
        /// <response code="404">The author with the specified ID was not found.</response>
        /// <response code="500">An error occurred while attempting to delete the author.</response>
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


        /// <summary>
        /// Creates a new author. Optionally, an image can be uploaded along with the author data.
        /// </summary>
        /// <param name="model">The model containing author data. An optional image can be included in the request.</param>
        /// <param name="image">An optional image file to be associated with the author.</param>
        /// <returns>Returns a Created response with the newly created author if successful.</returns>
        /// <response code="201">The author was successfully created.</response>
        /// <response code="400">The author data is null or invalid.</response>
        /// <response code="500">An error occurred while creating the author.</response>
        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor([FromForm] AuthorCreateModel model, IFormFile? image)
        {
            if (model == null)
            {
                return BadRequest("Author data is null.");
            }

            string? imageUrl = null;

            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine("wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imageUrl = $"/images/{fileName}";
            }

            // Convert model to entity
            var AuthorEntity = new BookstoreApi.Entities.Author
            {
                author_name = model.author_name,
                biography = model.biography,
                imageUrl = imageUrl

            };

            bscontext.Authors.Add(AuthorEntity);
            await bscontext.SaveChangesAsync();

            // Optionally: Return the created book with its ID
            //model.author_id = AuthorEntity.author_id;

            return CreatedAtAction(nameof(GetAuthors), new { id = AuthorEntity.author_id }, model);
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

        /// <summary>
        /// Updates an existing author. Optionally, an image can be uploaded to update the author's image.
        /// </summary>
        /// <param name="id">The unique identifier of the author to be updated.</param>
        /// <param name="updateModel">The updated author data.</param>
        /// <param name="image">The new image file to be uploaded. Optional.</param>
        /// <returns>Returns a NoContent status if successful, or an error response if the update fails.</returns>
        /// <response code="204">The author was successfully updated.</response>
        /// <response code="400">The author data is null or ID mismatch.</response>
        /// <response code="404">The author was not found.</response>
        /// <response code="500">An error occurred while updating the author.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromForm] AuthorUpdateModel updateModel, IFormFile? image)
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

            // Find the existing author
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
            if (image != null && image.Length > 0)
            {
                // Process the new image
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine("wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Update the image URL
                existingAuthor.imageUrl = $"/images/{fileName}";
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