using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiMotor.Business.Abstractions;
using ServiMotor.Business.Shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiMotor.Business.Models
{
    public class Extract : AggregateRoot, IAuditableEntity
    {
        public Extract()
        {
            this._id = ObjectId.GenerateNewId();
        }

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
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }

        internal bool HaveSameBranchOffice(Extract oldExtract)
        {
            return this.BranchOffice._id.Equals(oldExtract.BranchOffice._id);
        }

        internal void UpdateOrCreateResume(IDomainEvent domainEvent) => RaiseDomainEvent(domainEvent);
    }
}