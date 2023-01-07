using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAutores.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 8, ErrorMessage = "El campo {0} no debe de tener más de {1} carácteres.")]
        public string Nombre { get; set; }
        [Range(18,20)]
        [NotMapped]
        public int Edad { get; set; }
        [CreditCard]
        [NotMapped]
        public string TarjetaDeCredito { get; set; }
        [Url]
        [NotMapped]
        public string URL { get; set; }
        public List<Libro> Libros { get; set; }
    }
}
