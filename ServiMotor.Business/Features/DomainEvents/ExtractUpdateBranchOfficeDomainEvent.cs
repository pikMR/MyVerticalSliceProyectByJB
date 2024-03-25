using MongoDB.Bson;

namespace ServiMotor.Business.Features.DomainEvents;

public sealed record ExtractUpdateBranchOfficeDomainEvent(ObjectId Id, ObjectId OldBranchOfficeId, ObjectId NewBranchOfficeId, ObjectId BankId) : DomainEvent(Id);