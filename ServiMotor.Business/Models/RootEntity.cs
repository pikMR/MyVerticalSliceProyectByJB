using MongoDB.Bson;

namespace ServiMotor.Business.Models
{
    public class RootEntity
    {
        public ObjectId _id { get; set; }

        public RootEntity(ObjectId id)
        {
            _id = id;
        }

        public RootEntity()
        {

        }
    }
}
