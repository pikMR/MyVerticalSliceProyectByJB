using MongoDB.Bson;
using ServiMotor.Business.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ServiMotor.Business.Models
{
    public abstract class AggregateRoot : RootEntity
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public AggregateRoot(ObjectId id)
            : base(id)
        {
        }

        public AggregateRoot()
        {

        }

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
            _domainEvents.Add(domainEvent);
    }
}
