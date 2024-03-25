using MongoDB.Bson;
using ServiMotor.Business.Shared;

namespace ServiMotor.Business.Features.DomainEvents;
public abstract record DomainEvent(ObjectId Id) : IDomainEvent;

