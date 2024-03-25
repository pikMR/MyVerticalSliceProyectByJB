using MongoDB.Driver;
using System.Collections.Generic;

namespace ServiMotor.Business.Models
{
    public class Resume : RootEntity
    {
        public MongoDBRef IdBank { get; set; }

        public MongoDBRef IdBranchOffice { get; set; }

        public decimal BalanceBase { get; set; }

        public decimal BalanceFinal { get; set; }

        public HashSet<MongoDBRef> Extracts { get; set; }
    }
}
