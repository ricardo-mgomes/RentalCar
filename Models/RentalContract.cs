using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RentalCar.Models
{
    public class RentalContract
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O cliente é obrigatório.")]
        [Display(Name = "Cliente")]
        public int? ClientId { get; set; } 

        [Required(ErrorMessage = "O veículo é obrigatório.")]
        [Display(Name = "Veículo")]
        public int? VehicleId { get; set; }

        [ValidateNever]
        public Client? Client { get; set; }

        [ValidateNever]
        public Vehicle? Vehicle { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [Display(Name = "Data de Início")]
        public DateTime DataInicio { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "A data de fim é obrigatória.")]
        [Display(Name = "Data de Fim")]
        public DateTime DataFim { get; set; } 

        [Required(ErrorMessage = "A quilometragem é obrigatória.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quilometragem inválida.")]
        [Display(Name = "Quilometragem Inicial")]
        public int? QuilometragemInicial { get; set; } 
    }
}
