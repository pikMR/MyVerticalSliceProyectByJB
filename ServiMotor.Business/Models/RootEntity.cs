using MongoDB.Bson;

namespace ServiMotor.Business.Models
{
    public class RootEntity
    {
        public string Name { get; set; }

        public ObjectId _id { get; set; }

        public RootEntity(ObjectId id)
        {
            _id = id;
        }

        public RootEntity() { }
    }
}
