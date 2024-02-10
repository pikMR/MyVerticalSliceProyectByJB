using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiMotor.Business.Models
{
    public class Bank : RootEntity
    {
        [Required]
        [StringLength(8, ErrorMessage = "Unidad no puede tener mas de 8 caracteres.")]
        public string Name { get; set; }
    }
}
