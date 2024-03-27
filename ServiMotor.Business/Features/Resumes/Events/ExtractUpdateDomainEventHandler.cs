using AutoMapper;
using ServiMotor.Business.Features.DomainEvents;
using ServiMotor.Business.Models;
using ServiMotor.Business.Shared;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Business.Features.Resumes.Events
{
    public sealed class ExtractUpdateDomainEventHandler : IDomainEventHandler<ExtractUpdateDomainEvent>
    {
        private readonly IBaseRepository<Resume> _resumeRepository;
        private readonly IMapper _mapper;

        public ExtractUpdateDomainEventHandler(IBaseRepository<Resume> resumeRepository, IMapper mapper)
        {
            this._resumeRepository = resumeRepository;
            this._mapper = mapper;
        }

        public async Task Handle(ExtractUpdateDomainEvent notification, CancellationToken cancellationToken)
        {
            var resume = await this._resumeRepository.GetFirstAsync(x =>
                x.BranchOffice._id == notification.NewExtract.BranchOffice._id &&
                x.Bank._id == notification.NewExtract.Bank._id);

            var resumeExtract = resume.Extracts.FirstOrDefault(x => x._id.Equals(notification.NewExtract._id));

            if (resumeExtract != null)
            {
                resumeExtract.Update(notification.NewExtract.Name, notification.NewExtract.Balance);
                // TODO compute
                await this._resumeRepository.UpdateAsync(resume);
            }
        }
    }
}
