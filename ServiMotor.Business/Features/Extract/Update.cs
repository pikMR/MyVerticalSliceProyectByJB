using AutoMapper;
using FluentValidation;
using MediatR;
using ServiMotor.Business.Features.DomainEvents;
using ServiMotor.Business.Models;
using ServiMotor.Business.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Features.Extracts
{
    public class Update
    {
        public class Command : IRequest<string>
        {
            public string Id { get; set; }
            public string Description { get; set; }
            public Banks.Create.Command Bank { get; set; }
            public DateTime Date { get; set; }
            public decimal Balance { get; set; }
            public string Detail { get; set; }
            public BranchOffices.Create.Command BranchOffice { get; set; }

            public Command()
            {
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.Id).NotEmpty();
            }
        }

        public class CommandHandler : IRequestHandler<Command, string>
        {
            private readonly IBaseRepository<Extract> _repositoryExtract;
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;

            public CommandHandler(IBaseRepository<Extract> repository, IMapper mapper, IMediator mediator)
            {
                _repositoryExtract = repository;
                _mapper = mapper;
                _mediator = mediator;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                Extract newExtract = null;
                if (request.Id != null)
                {
                    var oldExtract = await _repositoryExtract.GetAsync(request.Id);
                    var bank = await _mediator.Send(request.Bank, cancellationToken);
                    var branchOffice = await _mediator.Send(request.BranchOffice, cancellationToken);
                    newExtract = _mapper.Map<Extract>(request);
                    newExtract.BranchOffice = branchOffice;
                    newExtract.Bank = bank;

                    if (!newExtract.BranchOffice._id.Equals(oldExtract.BranchOffice._id))
                    {
                        newExtract.UpdateResume(new ExtractUpdateBranchOfficeDomainEvent(newExtract._id, oldExtract.BranchOffice._id, newExtract.BranchOffice._id, newExtract.Bank._id));
                    }

                    await _repositoryExtract.UpdateAsync(newExtract);
                }

                return newExtract._id.ToString();
            }
        }
    }
}