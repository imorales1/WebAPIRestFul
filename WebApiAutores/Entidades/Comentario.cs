using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 500)]
        public string Contenido { get; set; }
        public int LibroId { get; set; }
        //Proppiedad de navegación
        public Libro Libro { get; set; }
    }
}
