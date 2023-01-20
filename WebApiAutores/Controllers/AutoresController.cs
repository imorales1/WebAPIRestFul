using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using WebApiAutores.Filtros;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async  Task<ActionResult<List<Autor>>> Get()
        {
            try
            {
                //return await context.Autores.Include(x => x.Libros).ToListAsync();
                return await context.Autores.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get([FromRoute] int id)
        {
            //var autor = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(a => a.Id == id);
            var autor = await context.Autores.FirstOrDefaultAsync(a => a.Id == id);
            if (autor == null)
            {
                return BadRequest($"El autor: {id}, no existe!");
            }

            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {
            var ExisteAutor = await context.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);

            if(ExisteAutor)
            {
                return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.Nombre}");
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);

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
