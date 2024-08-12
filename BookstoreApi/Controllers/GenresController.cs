using BookstoreApi.DBContext;
using BookstoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BookstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GenresController : ControllerBase
    {
        private readonly BookstoreContext bscontext;

        // Inject the logger in addition to the context
        private readonly ILogger<GenresController> _logger;

        public GenresController(BookstoreContext context, ILogger<GenresController> logger)
        {
            bscontext = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all genres.
        /// </summary>
        /// <returns>Returns a list of genres.</returns>
        /// <response code="200">Returns a list of all genres.</response>
        /// <response code="500">An error occurred while retrieving the genres.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            try
            {
                var genres = await bscontext.Genres.ToListAsync();

                // Return 200 OK with the list of genres
                return Ok(genres);
            }
            catch (System.Exception ex)
            {
                // Log the error and return 500 for internal server error
                _logger.LogError(ex, "An error occurred while retrieving the genres.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a genre by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the genre.</param>
        /// <returns>Returns the genre with the specified ID.</returns>
        /// <response code="200">Returns the genre with the specified ID.</response>
        /// <response code="404">If no genre is found with the specified ID.</response>
        /// <response code="500">An error occurred while retrieving the genre.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            try
            {
                var genre = await bscontext.Genres.FirstOrDefaultAsync(g => g.genre_id == id);

                if (genre == null)
                {
                    // Return 404 if the genre is not found
                    return NotFound();
                }

                // Return 200 OK with the genre
                return Ok(genre);
            }
            catch (System.Exception ex)
            {
                // Log the error and return 500 for internal server error
                _logger.LogError(ex, "An error occurred while retrieving the genre with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a genre by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the genre to be deleted.</param>
        /// <returns>Returns a status code indicating the result of the delete operation.</returns>
        /// <response code="204">Successfully deleted the genre.</response>
        /// <response code="404">If no genre is found with the specified ID.</response>
        /// <response code="500">An error occurred while deleting the genre.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            try
            {
                var genre = await bscontext.Genres.FindAsync(id);
                if (genre == null)
                {
                    // Return 404 if the book is not found
                    return NotFound(new { error = "Genre not found" });
                }

                bscontext.Genres.Remove(genre);
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
        /// Creates a new genre.
        /// </summary>
        /// <param name="genre">The genre data to be created.</param>
        /// <returns>Returns the created genre along with its unique identifier.</returns>
        /// <response code="201">Successfully created the genre.</response>
        /// <response code="400">If the provided genre data is null or invalid.</response>
        [HttpPost]
        public async Task<ActionResult<Genre>> CreateGenre(Genre genre)
        {
            if (genre == null)
            {
                return BadRequest("Genre is null.");
            }

            // Convert model to entity
            var GenreEntity = new BookstoreApi.Entities.Genre
            {
                genre_name = genre.genre_name

            };

            bscontext.Genres.Add(GenreEntity);
            await bscontext.SaveChangesAsync();

            // Optionally: Return the created book with its ID
            genre.genre_id = GenreEntity.genre_id;

            return CreatedAtAction(nameof(GetGenres), new { id = genre.genre_id }, genre);
        }



        private bool GenreExists(int id)
        {
            return bscontext.Genres.Any(e => e.genre_id == id);
        }


        /// <summary>
        /// Updates an existing genre.
        /// </summary>
        /// <param name="id">The unique identifier of the genre to update.</param>
        /// <param name="updateModel">The genre data to be updated.</param>
        /// <returns>Returns a status code indicating the result of the update operation.</returns>
        /// <response code="204">Successfully updated the genre.</response>
        /// <response code="400">If the provided genre data is null or the ID in the URL does not match the ID in the data.</response>
        /// <response code="404">If the genre with the specified ID is not found.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] Genre updateModel)
        {
            Console.WriteLine("here2");
            if (updateModel == null)
            {
                Console.WriteLine("here1");
                return BadRequest("Genre data is null.");
            }

            // Check if the ID from the route matches the ID in the update data
            if (id != updateModel.genre_id)
            {
                Console.WriteLine("here3" + updateModel.genre_id);
                return BadRequest("ID in the URL does not match the ID in the update data.");
            }

            // Find the existing Genre
            var existingGenre = await bscontext.Genres.FindAsync(id);
            if (existingGenre == null)
            {
                return NotFound();
            }

            // Update fields only if they are provided in the request


            if (!string.IsNullOrEmpty(updateModel.genre_name))
            {
                Console.WriteLine("here4" + updateModel.genre_name);
                existingGenre.genre_name = updateModel.genre_name;
            }






            try
            {
                // Save changes to the database
                await bscontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues
                if (!GenreExists(id))
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