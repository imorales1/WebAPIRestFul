using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{id:int}", Name = "obtenerLibro")]
        public async Task<ActionResult<LibroDTOConAutores>> Get([FromRoute] int id)
        {
            var libro = await context.Libros.Include(libroDb => libroDb.AutorLibro.OrderByDescending(x => x.Orden))
                .ThenInclude(x => x.Autor).FirstOrDefaultAsync(a => a.Id == id);
            if (libro == null) { return BadRequest($"El libro: {id}, no existe"); }

            return mapper.Map<LibroDTOConAutores>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCrecionDTO)
        {
            try
            {
                if(libroCrecionDTO.AutoresIds == null)
                {
                    return BadRequest("No se puede crear un libro sin autores");
                }

                var autoresIds = await context.Autores
                    .Where(autorBD => libroCrecionDTO.AutoresIds.Contains(autorBD.Id)).Select(x => x.Id).ToListAsync();

                if(libroCrecionDTO.AutoresIds.Count != autoresIds.Count)
                {
                    return BadRequest("No existe el autor enviado");
                }

                var libro = mapper.Map<Libro>(libroCrecionDTO);
                AsignarOrdenAutores(libro);

                context.Add(libro);
                await context.SaveChangesAsync();

                var libroDTO = mapper.Map<LibroDTO>(libro);
                return CreatedAtRoute("obtenerLibro", new { id = libro.Id }, libroDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDTO)
        {
            var libroDB = await context.Libros.Include(x => x.AutorLibro).FirstOrDefaultAsync(x => x.Id == id);

            if(libroDB == null)
            {
                return NotFound();
            }

            libroDB = mapper.Map(libroCreacionDTO, libroDB);
            AsignarOrdenAutores(libroDB);
            context.Update(libroDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.AutorLibro != null)
            {
                for (int i = 0; i < libro.AutorLibro.Count; i++)
                {
                    libro.AutorLibro[i].Orden = i;
                }
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }

            var libroDB = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);

            if(libroDB == null)
            {
                return NotFound();
            }

            var libroDTO = mapper.Map<LibroPatchDTO>(libroDB);

            patchDocument.ApplyTo(libroDTO, ModelState);

            var IsValid = TryValidateModel(libroDTO);
            if (!IsValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(libroDTO, libroDB);
            await context.SaveChangesAsync();

            return NoContent();
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
