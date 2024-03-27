using ServiMotor.Business.Features.DomainEvents;
using ServiMotor.Business.Models;
using ServiMotor.Business.Shared;
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
                x.BranchOffice._id == notification.Extract.BranchOffice._id &&
                x.Bank._id == notification.Extract.Bank._id);

            if (resume.DeleteResumeExtract(notification.Id))
            {
                // TODO compute()
                await this._resumeRepository.UpdateAsync(resume);
            }
        }
    }
}
