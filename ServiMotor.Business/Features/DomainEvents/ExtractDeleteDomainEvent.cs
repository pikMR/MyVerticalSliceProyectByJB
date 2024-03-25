using MongoDB.Bson;

namespace ServiMotor.Business.Features.DomainEvents;

public sealed record ExtractDeleteDomainEvent(ObjectId Id, ObjectId BranchOfficeId, ObjectId BankId) : DomainEvent(Id);

