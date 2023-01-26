using WebApiAutores.Entidades;

namespace WebApiAutores.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public List<AutorDTO> Autores { get; set; }

        //public List<Comentario> Comentarios { get; set; }
    }
}
