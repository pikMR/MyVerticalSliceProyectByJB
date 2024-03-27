using ServiMotor.Business.Models;

namespace ServiMotor.Business.Features.DomainEvents;

public sealed record ExtractUpdateBranchOfficeDomainEvent(Extract NewExtract, Extract OldExtract) : DomainEvent(NewExtract._id);