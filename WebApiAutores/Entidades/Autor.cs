using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Autor: IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(maximumLength: 50, ErrorMessage = "El campo {0} no debe de tener más de {1} carácteres.")]
        //[PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        [Range(10,85)]
        [NotMapped]
        public int Edad { get; set; }
        //[CreditCard]
        [NotMapped]
        public string TarjetaDeCredito { get; set; }
        [Url]
        [NotMapped]
        public string URL { get; set; }
        public List<Libro> Libros { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if(primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayúscula", new string[] { nameof(Nombre)});
                }
            }
        }
    }
}
