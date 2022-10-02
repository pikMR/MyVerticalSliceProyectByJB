using System.ComponentModel.DataAnnotations;

namespace ServiMotor.Business.Models
{
    public class Oil : RootEntity
    {
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Unidad")]
        [StringLength(8, ErrorMessage = "Unidad no puede tener mas de 8 caracteres.")]
        public string Unit { get; set; }
    }
}