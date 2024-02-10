using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ServiMotor.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiMotor.Business.Features.Resumes
{
    public class AvailableBalance
    {
        public BranchOffice BranchOffice { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Amount { get; set; }
    }
}
