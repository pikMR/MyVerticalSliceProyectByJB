using MongoDB.Driver;
using ServiMotor.Business.Features.DomainEvents;
using ServiMotor.Business.Models;
using ServiMotor.Business.Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Business.Features.Resumes.Events
{
    public sealed class ExtractCreateDomainEventHandler : IDomainEventHandler<ExtractCreateDomainEvent>
    {
        private readonly IBaseRepository<Resume> _resumeRepository;

        public ExtractCreateDomainEventHandler(IBaseRepository<Extract> extractRepository, IBaseRepository<Resume> resumeRepository)
        {
            this._resumeRepository = resumeRepository;
        }

        public async Task Handle(ExtractCreateDomainEvent notification, CancellationToken cancellationToken)
        {
            var resume = await this._resumeRepository.GetFirstAsync(x =>
                x.IdBranchOffice.Id == notification.BranchOfficeId &&
                x.IdBank.Id == notification.BankId);

            if (resume == null)
            {
                // create resume and add extract to resume
                resume = new Resume()
                {
                    IdBank = new MongoDBRef("bank", notification.BankId),
                    IdBranchOffice = new MongoDBRef("branchOffice", notification.BranchOfficeId),
                    BalanceBase = 0,
                    BalanceFinal = 0,
                    Extracts = new HashSet<MongoDBRef>()
                    {
                        new MongoDBRef("extract", notification.Id)
                    }
                };

                await this._resumeRepository.CreateAsync(resume);
            }
            else
            {
                // add extract to resume
                resume.Extracts.Add(new MongoDBRef("extract", notification.Id));
                await this._resumeRepository.UpdateAsync(resume);
            }
        }
    }
}
