﻿using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo nombre es requerido")]
        [StringLength(maximumLength: 5, ErrorMessage = "El campo {0} no debe de tener más de {1} caracteres.")]
        public string Titulo { get; set; }
        public int AutorId { get; set; }
        public Autor Autor { get; set; }
    }
}