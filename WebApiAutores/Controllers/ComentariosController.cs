using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using WebApiAutores.Migrations;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ComentariosController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {
            try
            {
                var comentarios = await context.Comentarios.Where(comentarioDB => comentarioDB.LibroId == libroId).ToListAsync();

                return mapper.Map<List<ComentarioDTO>>(comentarios);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}", Name = "obtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetId(int Id)
        {
            var comentario = await context.Comentarios.FirstOrDefaultAsync(comentarioDB => comentarioDB.Id == Id);
            if(comentario == null)
            {
                return NotFound();
            }
            return mapper.Map<ComentarioDTO>(comentario);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            try
            {
                var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = emailClaim.Value;
                var usuario = await userManager.FindByEmailAsync(email);
                var usuarioId = usuario.Id;
                var existeLibro = await context.Libros.AnyAsync(libro => libro.Id == libroId);
                if(!existeLibro)
                {
                    return NotFound();
                }

                var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
                comentario.LibroId = libroId;
                comentario.UsuarioId = usuarioId;
                context.Add(comentario);
                await context.SaveChangesAsync();

                var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
                return CreatedAtRoute("obtenerComentario", new { Id = comentario.Id, libroId = comentario.LibroId }, comentarioDTO);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int libroId, int id, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await context.Libros.AnyAsync(libroDb => libroDb.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var existeComentario = await context.Comentarios.AnyAsync(comentarioDB => comentarioDB.Id == id);
            if (!existeComentario)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.Id = id;
            comentario.LibroId = libroId;

            context.Update(comentario);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
