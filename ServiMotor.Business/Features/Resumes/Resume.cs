using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;

namespace ServiMotor.Business.Features.Resumes
{
    public class Resume
    {
        public IEnumerable<AvailableBalance> AvailableBalances { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalAmount { get; set; }
    }
}
