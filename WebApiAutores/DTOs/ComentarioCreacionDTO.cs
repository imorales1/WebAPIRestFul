using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs
{
    public class ComentarioCreacionDTO
    {
        [StringLength(maximumLength: 500)]
        [PrimeraLetraMayuscula]
        public string Contenido { get; set; }
    }
}
