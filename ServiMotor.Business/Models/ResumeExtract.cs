using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiMotor.Business.Models
{
    public class ResumeExtract : RootEntity
    {
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Balance { get; set; }

        internal void Update(string name, decimal balance)
        {
            this.Name = name;
            this.Balance = balance;
        }
    }
}
