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

        public GenresController(BookstoreContext context)
        {
            bscontext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            return Ok(await bscontext.Genres.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetGenre(int id)
        {
            var genre = await bscontext.Genres.FirstOrDefaultAsync(g => g.genre_id == id);

            if (genre == null)
            {
                // Return 404 if the book is not found
                return NotFound();
            }

            return Ok(genre);
        }

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



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] Genre updateModel)
        {
            if (updateModel == null)
            {
                return BadRequest("Genre data is null.");
            }

            // Check if the ID from the route matches the ID in the update data
            if (id != updateModel.genre_id)
            {
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