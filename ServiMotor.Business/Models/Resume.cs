using MongoDB.Bson;
using ServiMotor.Business.Abstractions;
using System;
using System.Collections.Generic;

namespace ServiMotor.Business.Models
{
    public class Resume : RootEntity, IAuditableEntity
    {
        public Resume(Extract extract)
        {
            Bank = extract.Bank;
            BranchOffice = extract.BranchOffice;
            BalanceBase = 0;
            BalanceFinal = 0;
            Extracts = new HashSet<ResumeExtract>()
            {
                new ResumeExtract()
                {
                    Name= extract.Name,
                    Balance= extract.Balance,
                    _id = extract._id
                }
            };

            CreatedOnUtc = DateTime.UtcNow;
        }

        public RootEntity Bank { get; set; }
        public RootEntity BranchOffice { get; set; }
        public decimal BalanceBase { get; set; }
        public decimal BalanceFinal { get; set; }
        public HashSet<ResumeExtract> Extracts { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }

        internal void AddResumeExtract(ResumeExtract resumeExtract)
        {
            if (Extracts != null)
            {
                this.Extracts.Add(resumeExtract);
            }
            else
            {
                this.Extracts = new HashSet<ResumeExtract> { resumeExtract };
            }

            ModifiedOnUtc = DateTime.UtcNow;
        }

        public bool DeleteResumeExtract(ObjectId id)
        {
            if (Extracts != null)
            {
                this.Extracts.RemoveWhere(x => x._id == id);
                ModifiedOnUtc = DateTime.UtcNow;
                return true;
            }

            return false;
        }
    }
}
