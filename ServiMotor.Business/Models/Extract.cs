using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiMotor.Business.Models
{
    public class Extract : RootEntity
    {
        [Required]
        [Display(Name = "Descripcion")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Sucursal")]
        public BranchOffice BranchOffice { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }

        [Display(Name = "Saldo")]
        [Required]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Balance { get; set; }

        [Display(Name = "Detalle")]
        public string Detail { get; set; }

        [Display(Name = "Banco")]
        public Bank Bank { get; set; }
    }
}