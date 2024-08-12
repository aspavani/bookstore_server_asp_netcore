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

        // Inject the logger in addition to the context
        private readonly ILogger<BooksController> _logger;

        public BooksController(BookstoreContext context, ILogger<BooksController> logger)
        {
            bscontext = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all books from the bookstore, including details about the author and genre.
        /// </summary>
        /// <returns>
        /// An <see cref="ActionResult{IEnumerable{Book}}"/> containing a list of books along with their associated authors and genres.
        /// The response will be a JSON array of book objects with author and genre information.
        /// </returns>
        /// <response code="200">OK - Returns a list of books with author and genre details.</response>
        /// <response code="500">Internal Server Error - An error occurred while processing the request.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            // return Ok(await bscontext.Books.Include(b => b.Author).Include(b => b.Genre).ToListAsync());
            try
            {
                var books = await bscontext.Books
                    .Include(b => b.Author)
                    .Include(b => b.Genre)
                    .ToListAsync();
                _logger.LogInformation("Retrieved all books successfully.");
                return Ok(books);
            }
            catch (Exception ex)
            {
                // Log the exception 
                _logger.LogError(ex, "An error occurred while retrieving the books.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the books.");
            }
        }


        /// <summary>
        /// Retrieves a single book by its unique identifier, including details about the author and genre.
        /// </summary>
        /// <param name="id">The unique identifier of the book to retrieve.</param>
        /// <returns>
        /// An <see cref="ActionResult{Book}"/> containing the requested book with its associated author and genre details.
        /// If the book is not found, a 404 Not Found response will be returned.
        /// </returns>
        /// <response code="200">OK - Returns the book with author and genre details.</response>
        /// <response code="404">Not Found - The book with the specified ID was not found.</response>
        /// <response code="500">Internal Server Error - An error occurred while processing the request.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                var book = await bscontext.Books.Include(b => b.Author).Include(b => b.Genre).FirstOrDefaultAsync(b => b.book_id == id);

                if (book == null)
                {
                    _logger.LogWarning("Book with ID {Id} not found.", id);
                    // Return 404 if the book is not found
                    return NotFound();
                }
                _logger.LogInformation("Retrieved book with ID {Id} successfully.", id);
                return Ok(book);
            }
            catch (Exception ex)
            {
                // Log the exception 
                _logger.LogError(ex, "An error occurred while retrieving the book with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the book.");
            }
        }


        /// <summary>
        /// Deletes a book by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book to be deleted.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the result of the delete operation.
        /// If the book is not found, a 404 Not Found response will be returned.
        /// If the deletion is successful, a 204 No Content response will be returned.
        /// </returns>
        /// <response code="204">No Content - The book was successfully deleted.</response>
        /// <response code="404">Not Found - The book with the specified ID was not found.</response>
        /// <response code="500">Internal Server Error - An error occurred while processing the request.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await bscontext.Books.FindAsync(id);
                if (book == null)
                {
                    _logger.LogWarning("Attempted to delete book with ID {Id}, but it was not found.", id);
                    // Return 404 if the book is not found
                    return NotFound(new { error = "Book not found" });
                }

                bscontext.Books.Remove(book);
                await bscontext.SaveChangesAsync();

                _logger.LogInformation("Book with ID {Id} deleted successfully.", id);
                // Return 204 for a successful deletion with no content
                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the book with ID {Id}.", id);
                // Return 500 for an internal server error with the exception message
                return StatusCode(500, new { error = ex.Message });
            }
        }


        /// <summary>
        /// Creates a new book in the bookstore.
        /// </summary>
        /// <param name="book">The book object containing the details of the book to be created. It should include the title, price, publication date, image URL, author ID, and genre ID.</param>
        /// <returns>
        /// An <see cref="ActionResult{Book}"/> representing the result of the create operation.
        /// If the creation is successful, a 201 Created response with the location of the new resource and the created book details will be returned.
        /// If the provided book object is null, a 400 Bad Request response will be returned.
        /// </returns>
        /// <response code="201">Created - The book was successfully created. The response includes the location of the new book and the created book details.</response>
        /// <response code="400">Bad Request - The book object provided in the request is null or invalid.</response>
        /// <response code="500">Internal Server Error - An error occurred while processing the request.</response>
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            if (book == null)
            {
                _logger.LogWarning("Attempted to create a book but the book object was null.");

                return BadRequest("Book is null.");
            }

            try
            {
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
                _logger.LogInformation("Created new book with ID {Id}.", book.book_id);
                return CreatedAtAction(nameof(GetBooks), new { id = book.book_id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new book.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the book.");
            }
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

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">The unique identifier of the book to be updated.</param>
        /// <param name="updateModel">The model containing updated book data. Fields not provided will not be updated.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ApiExplorerSettings(GroupName = "v1")]
        /// <param name="id">The unique identifier of the book to be updated.</param>
        /// <param name="updateModel">The model containing updated book data.</param>
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateModel updateModel)
        {
            if (updateModel == null)
            {
                _logger.LogWarning("Attempted to update a book but the update model was null.");
                return BadRequest("Book data is null.");
            }

            // Check if the ID from the route matches the ID in the update data
            if (id != updateModel.book_id)
            {
                _logger.LogWarning("ID in the URL ({Id}) does not match the ID in the update data ({UpdateId}).", id, updateModel.book_id);
                return BadRequest("ID in the URL does not match the ID in the update data.");
            }



            // Find the existing book
            var existingBook = await bscontext.Books.FindAsync(id);
            if (existingBook == null)
            {
                _logger.LogWarning("Book with ID {Id} not found for update.", id);
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
                    _logger.LogWarning("Book with ID {Id} not found during update.", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation("Updated book with ID {Id} successfully.", id);
            return NoContent();
        }



    }
}