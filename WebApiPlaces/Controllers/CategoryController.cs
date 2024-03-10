using ClassLibraryPlaces.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApiPlaces.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly PLACESDBContext context;
        public CategoryController(PLACESDBContext context)
        {
            this.context = context;
        }
        [HttpGet] // api/Autor  RUTA POR DEFECTO
        public async Task<ActionResult<List<Category>>> Get()
        {
            var categories = await context.Categories.ToListAsync();
            return categories;
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }
        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            if(ModelState.IsValid)
            {
                context.Categories.Add(category);
                await context.SaveChangesAsync();
                var categoriaSimplificado = new
                {
                    category.Id,
                    category.Title
                };
                return Ok(categoriaSimplificado);
            }
            return BadRequest(ModelState);

            //return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }
  
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category updatedCategory)
        {
            try
            {
                if (id != updatedCategory.Id)
                    return BadRequest("El ID de la categoría no coincide con el proporcionado en la ruta");

                var existingCategory = await context.Categories.FindAsync(id);
                if (existingCategory == null)
                    return NotFound($"Categoría con ID = {id} no encontrada");

                // Actualiza las propiedades relevantes
                existingCategory.Title = updatedCategory.Title;

                // Guarda los cambios en la base de datos
                await context.SaveChangesAsync();

                return Ok(existingCategory); // Devuelve la categoría actualizada
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al actualizar los datos");
            }
        }
        
        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}

