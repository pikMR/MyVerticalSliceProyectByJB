using MongoDB.Driver;
using ServiMotor.Business.Features.DomainEvents;
using ServiMotor.Business.Models;
using ServiMotor.Business.Shared;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Business.Features.Resumes.Events
{
    public sealed class ExtractDeleteInResumeDomainEventHandler
        : IDomainEventHandler<ExtractDeleteDomainEvent>
    {
        private readonly IBaseRepository<Extract> _extractRepository;
        private readonly IBaseRepository<Resume> _resumeRepository;

        public ExtractDeleteInResumeDomainEventHandler(IBaseRepository<Extract> extractRepository, IBaseRepository<Resume> resumeRepository)
        {
            this._extractRepository = extractRepository;
            this._resumeRepository = resumeRepository;
        }

        public async Task Handle(ExtractDeleteDomainEvent notification, CancellationToken cancellationToken)
        {
            var resume = await this._resumeRepository.GetFirstAsync(x =>
                x.IdBranchOffice.Id == notification.BranchOfficeId &&
                x.IdBank.Id == notification.BankId);

            var extractRefToRemove = resume.Extracts.FirstOrDefault(extractRef => extractRef.Id == notification.Id);

            if (resume.Extracts.Remove(extractRefToRemove))
            {
                await this._resumeRepository.UpdateAsync(resume);
            }
        }
    }
}
