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

        //[HttpGet]
        //public async Task<ActionResult <List<Libro>>> Get()
        //{
        //    return await context.Libros.ToListAsync();
        //}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            return await context.Libros.Include(p => p.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

            if(!existeAutor)
            {
                return BadRequest($"El autor {libro.AutorId} no existe!");
            }
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
