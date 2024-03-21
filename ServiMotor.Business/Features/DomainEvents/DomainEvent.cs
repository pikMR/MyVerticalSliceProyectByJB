using MongoDB.Bson;
using ServiMotor.Business.Models;

namespace ServiMotor.Business.Features.DomainEvents;
public abstract record DomainEvent(ObjectId Id) : IDomainEvent;

