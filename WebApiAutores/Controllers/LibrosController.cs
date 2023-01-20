using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController:ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get([FromRoute] int id)
        {
            //var libro = await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(a => a.Id == id);
            //if(libro == null) { return BadRequest($"El libro: {id}, no existe"); }

            //return await context.Libros.Include(p => p.Autor).FirstOrDefaultAsync(x => x.Id == id);
            return await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        //public async Task<ActionResult> Post(Libro libro)
        //{
        //    try
        //    {
        //        //var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

        //        if (!existeAutor)
        //        {
        //            return BadRequest($"El autor {libro.AutorId} no existe!");
        //        }
        //        context.Add(libro);
        //        await context.SaveChangesAsync();
        //        return Ok();
        //    }catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Autor autor)
        {
            if (autor.Id != id)
            {
                return BadRequest("El id del libro no coincide con ninguno");
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return BadRequest("El id ingresado no existe");
            }

            context.Remove(new Libro() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
