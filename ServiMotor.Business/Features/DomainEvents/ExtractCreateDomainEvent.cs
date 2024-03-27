using ServiMotor.Business.Models;

namespace ServiMotor.Business.Features.DomainEvents;

public sealed record ExtractCreateDomainEvent(Extract Extract) : DomainEvent(Extract._id);

