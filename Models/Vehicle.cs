using System;
using System.ComponentModel.DataAnnotations;

namespace RentalCar.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A marca é obrigatória.")]
        [Display(Name = "Marca")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [Display(Name = "Modelo")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "A matrícula é obrigatória.")]
        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }

        [Required(ErrorMessage = "O ano de fabrico é obrigatório.")]
        [Range(1900, int.MaxValue, ErrorMessage = "Ano inválido.")]
        [Display(Name = "Ano de Fabrico")]
        public int? AnoFabricacao { get; set; }

        [Required(ErrorMessage = "O tipo de combustível é obrigatório.")]
        [Display(Name = "Combustível")]
        public CombustivelTipo? Combustivel { get; set; } = null;

        public bool EstaAlugado { get; set; }
    }
}
