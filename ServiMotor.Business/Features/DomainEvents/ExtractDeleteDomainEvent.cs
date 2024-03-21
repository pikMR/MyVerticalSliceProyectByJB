using MongoDB.Bson;

namespace ServiMotor.Business.Features.DomainEvents;

public sealed record ExtractDeleteDomainEvent(ObjectId Id, decimal Balance) : DomainEvent(Id);

