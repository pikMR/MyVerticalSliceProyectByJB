using MongoDB.Bson;
using System;

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
