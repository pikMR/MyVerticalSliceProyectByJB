using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiMotor.Business.Features.Resumes
{
    public class Resume
    {
        public IEnumerable<AvailableBalance> AvailableBalances { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalAmount { get; set; }
    }
}
