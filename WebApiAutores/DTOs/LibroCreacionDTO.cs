using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs
{
    public class LibroCreacionDTO
    {
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 40)]
        public string Titulo { get; set; }

    }
}
