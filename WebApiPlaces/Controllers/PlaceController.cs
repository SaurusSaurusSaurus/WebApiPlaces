using ClassLibraryPlaces.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApiPlaces.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly PLACESDBContext context; 

        public PlaceController(PLACESDBContext context)
        {
            this.context = context;
        
        }

        // GET: api/places
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlaces()
        {
            return await context.Places.ToListAsync();
        }

        // GET: api/places/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Place>> GetPlace(int id)
        {
            var place = await context.Places.FindAsync(id);

            if (place == null)
            {
                return NotFound();
            }

            return place;
        }
        // POST: api/places
        [HttpPost]
        public async Task<ActionResult<Place>> CreatePlace(Place place)
        {
            context.Places.Add(place);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlace), new { id = place.Id }, place);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlace(int id, Place updatedPlace)
        {
            if (id != updatedPlace.Id)
            {
                return BadRequest();
            }

            var existingPlace = await context.Places.FindAsync(id);
            if (existingPlace == null)
            {
                return NotFound();
            }

            // Actualiza solo los campos necesarios
            existingPlace.Latitude = updatedPlace.Latitude;
            existingPlace.Longitute = updatedPlace.Longitute;
            existingPlace.ImageUrl = updatedPlace.ImageUrl;
            existingPlace.Description = updatedPlace.Description;
            existingPlace.Title = updatedPlace.Title;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(id))
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
        // DELETE: api/places/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlace(int id)
        {
            var place = await context.Places.FindAsync(id);
            if (place == null)
            {
                return NotFound();
            }

            context.Places.Remove(place);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlaceExists(int id)
        {
            return context.Places.Any(p => p.Id == id);
        }

    }
}
