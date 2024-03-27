using AutoMapper;
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
        private readonly IBaseRepository<Resume> _resumeRepository;
        private readonly IMapper _mapper;

        public ExtractUpdateBranchOfficeDomainEventHandler(IBaseRepository<Resume> resumeRepository, IMapper mapper)
        {
            this._resumeRepository = resumeRepository;
            this._mapper = mapper;
        }

        public async Task Handle(ExtractUpdateBranchOfficeDomainEvent notification, CancellationToken cancellationToken)
        {
            var oldResume = await this._resumeRepository.GetFirstAsync(x =>
                x.BranchOffice._id == notification.OldExtract.BranchOffice._id &&
                x.Bank._id == notification.OldExtract.Bank._id);

            var newResume = await this._resumeRepository.GetFirstAsync(x =>
                x.BranchOffice._id == notification.NewExtract.BranchOffice._id &&
                x.Bank._id == notification.NewExtract.Bank._id);

            if (newResume == null)
            {
                newResume = new Resume(notification.NewExtract);
                await this._resumeRepository.CreateAsync(newResume);
            }
            else
            {
                var extractRefToRemove = oldResume.Extracts.FirstOrDefault(extractRef => extractRef._id == notification.Id);

                if (oldResume.Extracts.Remove(extractRefToRemove))
                {
                    // TODO OldResume.compute()
                    await this._resumeRepository.UpdateAsync(oldResume);
                }

                if (newResume.Extracts.Add(_mapper.Map<ResumeExtract>(notification.NewExtract)))
                {
                    // TODO OldResume.compute()
                    await this._resumeRepository.UpdateAsync(newResume);
                }
            }
        }
    }
}
