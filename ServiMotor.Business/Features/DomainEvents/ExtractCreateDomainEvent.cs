using MongoDB.Bson;

namespace ServiMotor.Business.Features.DomainEvents;

public sealed record ExtractCreateDomainEvent(ObjectId Id, decimal Balance) : DomainEvent(Id);

