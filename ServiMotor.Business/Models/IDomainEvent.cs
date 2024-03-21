using MediatR;
using MongoDB.Bson;

namespace ServiMotor.Business.Models
{
    public interface IDomainEvent : INotification
    {
        public ObjectId Id { get; init; }
    }
}
