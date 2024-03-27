using System;

namespace ServiMotor.Business.Abstractions
{
    public interface IAuditableEntity
    {
        DateTime CreatedOnUtc { get; set; }

        DateTime? ModifiedOnUtc { get; set; }
    }
}
