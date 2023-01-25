using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<LibroDTO>>> Get()
        {
            try
            {
                var libros = await context.Libros.Include(libroBD => libroBD.Comentarios).ToListAsync();
                return mapper.Map<List<LibroDTO>>(libros);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get([FromRoute] int id)
        {
            var libro = await context.Libros.Include(x => x.Comentarios).FirstOrDefaultAsync(a => a.Id == id);
            if (libro == null) { return BadRequest($"El libro: {id}, no existe"); }

            return mapper.Map<LibroDTO>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCrecionDTO)
        {
            try
            {
                //var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

                //if (!existeAutor)
                //{
                //    return BadRequest($"El autor {libro.AutorId} no existe!");
                //}

                var libro = mapper.Map<Libro>(libroCrecionDTO);
                context.Add(libro);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
