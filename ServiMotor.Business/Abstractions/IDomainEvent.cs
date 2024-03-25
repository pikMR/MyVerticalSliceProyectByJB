using MediatR;
using MongoDB.Bson;

namespace ServiMotor.Business.Shared
{
    public interface IDomainEvent : INotification
    {
        public ObjectId Id { get; init; }
    }
}
