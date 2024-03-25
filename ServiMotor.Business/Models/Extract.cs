using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiMotor.Business.Features.DomainEvents;
using ServiMotor.Business.Shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiMotor.Business.Models
{
    public class Extract : AggregateRoot
    {
        public Extract()
        {
            this._id = ObjectId.GenerateNewId();
        }

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

        internal void UpdateResume(IDomainEvent extractCreateDomainEvent) => RaiseDomainEvent(extractCreateDomainEvent);

        internal void UpdateCreateResume(ExtractCreateDomainEvent extractCreateDomainEvent) => RaiseDomainEvent(extractCreateDomainEvent);
    }
}