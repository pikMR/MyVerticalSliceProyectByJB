using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServiMotor.Business.Models
{
    public class BranchOffice : RootEntity
    {
        public BranchOffice(string branchOfficeName)
        {
            this._id = ObjectId.GenerateNewId();
            this.Name = branchOfficeName;
        }

        public BranchOffice()
        {

        }

        [Required]
        [StringLength(16, ErrorMessage = "Unidad no puede tener mas de 16 caracteres.")]
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BranchOffice office &&
                   _id.Equals(office._id) &&
                   Name == office.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, Name);
        }

        internal void SetId(string idBranchOffice)
        {
            if (!string.IsNullOrEmpty(idBranchOffice))
            {
                this._id = ObjectId.Parse(idBranchOffice);
            }
        }
    }
}
