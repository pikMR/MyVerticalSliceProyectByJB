using MongoDB.Bson;
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
        [StringLength(16, ErrorMessage = "Unidad no puede tener mas de 16 caracteres.")]
        public string Name { get; set; }

        public Bank() { }

        public Bank(string bankName)
        {
            this._id = ObjectId.GenerateNewId();
            this.Name = bankName;
        }

        public override bool Equals(object obj)
        {
            return obj is Bank bank &&
                   _id.Equals(bank._id) &&
                   Name == bank.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, Name);
        }

        internal void SetId(string idBank)
        {
            if (!string.IsNullOrEmpty(idBank))
            {
                this._id = ObjectId.Parse(idBank);
            }
        }
    }
}
