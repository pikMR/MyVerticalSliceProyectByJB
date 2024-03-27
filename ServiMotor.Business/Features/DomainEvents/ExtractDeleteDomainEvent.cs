using ServiMotor.Business.Models;

namespace ServiMotor.Business.Features.DomainEvents;

public sealed record ExtractDeleteDomainEvent(Extract Extract) : DomainEvent(Extract._id);

