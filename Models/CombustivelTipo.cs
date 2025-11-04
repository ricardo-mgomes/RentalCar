using System.ComponentModel.DataAnnotations;

namespace RentalCar.Models
{
    public enum CombustivelTipo
    {
        [Display(Name = "Gasolina")]
        Gasolina,

        [Display(Name = "Gasóleo")]
        Gasoleo,

        [Display(Name = "Hidrogénio")]
        Hidrogenio,

        [Display(Name = "Elétrico")]
        Eletrico
    }
}
