using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ServiMotor.Business.Models;

namespace ServiMotor.Business.Features.Resumes
{
    public class AvailableBalance
    {
        public BranchOffice BranchOffice { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Amount { get; set; }
    }
}
