using ServiMotor.Business.Models;

namespace ServiMotor.Business.Features.DomainEvents;

public sealed record ExtractUpdateDomainEvent(Extract NewExtract, Extract OldExtract) : DomainEvent(NewExtract._id);