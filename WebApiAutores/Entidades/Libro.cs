using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 40)]
        public string Titulo { get; set; }
    }
}
