using MongoDB.Driver;
using ServiMotor.Business.Features.DomainEvents;
using ServiMotor.Business.Models;
using ServiMotor.Business.Shared;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Business.Features.Resumes.Events
{
    public sealed class ExtractUpdateBranchOfficeDomainEventHandler : IDomainEventHandler<ExtractUpdateBranchOfficeDomainEvent>
    {
        private readonly IBaseRepository<Extract> _extractRepository;
        private readonly IBaseRepository<Resume> _resumeRepository;

        public ExtractUpdateBranchOfficeDomainEventHandler(IBaseRepository<Extract> extractRepository, IBaseRepository<Resume> resumeRepository)
        {
            this._extractRepository = extractRepository;
            this._resumeRepository = resumeRepository;
        }

        public async Task Handle(ExtractUpdateBranchOfficeDomainEvent notification, CancellationToken cancellationToken)
        {
            var OldResume = await this._resumeRepository.GetFirstAsync(x =>
                x.IdBranchOffice.Id == notification.OldBranchOfficeId &&
                x.IdBank.Id == notification.BankId);

            var NewResume = await this._resumeRepository.GetFirstAsync(x =>
                x.IdBranchOffice.Id == notification.NewBranchOfficeId &&
                x.IdBank.Id == notification.BankId);

            var extractRefToRemove = OldResume.Extracts.FirstOrDefault(extractRef => extractRef.Id == notification.Id);

            if (OldResume.Extracts.Remove(extractRefToRemove))
            {
                await this._resumeRepository.UpdateAsync(OldResume);
            }

            if (NewResume.Extracts.Add(new MongoDBRef("extract", notification.Id)))
            {
                await this._resumeRepository.CreateAsync(NewResume);
            }
        }
    }
}
