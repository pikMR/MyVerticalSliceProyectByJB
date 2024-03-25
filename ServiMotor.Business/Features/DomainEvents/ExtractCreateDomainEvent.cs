using MongoDB.Bson;

namespace ServiMotor.Business.Features.DomainEvents;

public sealed record ExtractCreateDomainEvent(ObjectId Id, ObjectId BranchOfficeId, ObjectId BankId) : DomainEvent(Id);

