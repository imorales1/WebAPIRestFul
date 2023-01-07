using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutoresController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        } 

        [HttpGet]
        [HttpGet("listado")]
        [HttpGet("/listado")]
        public async  Task<ActionResult<List<Autor>>> Get()
        {
            try
            {
                return await context.Autores.Include(x => x.Libros).ToListAsync();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //public async Task<ActionResult <List<Autor>>> Get()
        //{
        //    //return new List<Autor>()
        //    //{
        //    //    new Autor() { Id = 1, Nombre = "Alejandro"},
        //    //    new Autor() { Id = 2, Nombre = "Astrid"}
        //    //};

        //    return await context.Autores.Include(x => x.Libros).ToListAsync();
        //}

        [HttpGet("primero")]  //api/autores/primero/?nombre=valor&apellido=valor
        public async Task<ActionResult<Autor>> PrimerAutor([FromHeader] int miValor, [FromQuery] string nombre)
        {
            return await context.Autores.FirstOrDefaultAsync();
        }

        //[HttpGet("{id:int}")]
        //public IActionResult Get(int id)
        //{
        //    //return await context.Autores.FirstOrDefaultAsync();
        //    var autor = context.Autores.FirstOrDefault(a => a.Id == id);
        //    if (autor == null)
        //    {
        //        //return BadRequest($"El autor: {id}, no existe!");
        //    }

        //    return Ok(7);
        //}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get([FromRoute] int id)
        {
            //return await context.Autores.FirstOrDefaultAsync();
            var autor = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(a => a.Id == id);
            if (autor == null)
            {
                return BadRequest($"El autor: {id}, no existe!");
            }

            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Autor autor)
        {
            if(autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el ID ingresado");
            }

            //context.Autores.Find(id);
            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if(!existe)
            {
                return BadRequest("El id ingresado no existe");
            }

            context.Remove(new Autor() { Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
