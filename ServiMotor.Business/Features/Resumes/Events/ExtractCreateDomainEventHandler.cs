using AutoMapper;
using ServiMotor.Business.Features.DomainEvents;
using ServiMotor.Business.Models;
using ServiMotor.Business.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Business.Features.Resumes.Events
{
    public sealed class ExtractCreateDomainEventHandler : IDomainEventHandler<ExtractCreateDomainEvent>
    {
        private readonly IBaseRepository<Resume> _resumeRepository;
        private readonly IMapper _mapper;

        public ExtractCreateDomainEventHandler(IBaseRepository<Resume> resumeRepository, IMapper mapper)
        {
            this._mapper = mapper;
            this._resumeRepository = resumeRepository;
        }

        public async Task Handle(ExtractCreateDomainEvent notification, CancellationToken cancellationToken)
        {
            var resume = await this._resumeRepository.GetFirstAsync(x =>
                x.BranchOffice._id == notification.Extract.BranchOffice._id &&
                x.Bank._id == notification.Extract.Bank._id);

            if (resume == null)
            {
                resume = new Resume(notification.Extract);
                // TODO compute();
                await this._resumeRepository.CreateAsync(resume);
            }
            else
            {
                // add extract to resume
                resume.AddResumeExtract(this._mapper.Map<ResumeExtract>(notification.Extract));
                // TODO compute();
                await this._resumeRepository.UpdateAsync(resume);
            }
        }
    }
}
