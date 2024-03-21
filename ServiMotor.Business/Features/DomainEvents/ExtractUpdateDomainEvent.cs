using MongoDB.Bson;

namespace ServiMotor.Business.Features.DomainEvents;

public sealed record ExtractUpdateDomainEvent(ObjectId Id, decimal Balance) : DomainEvent(Id);