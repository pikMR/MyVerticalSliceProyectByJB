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
            public string Name { get; set; }
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

                    if (!newExtract.HaveSameBranchOffice(oldExtract))
                    {
                        await _mediator.Publish(new ExtractUpdateBranchOfficeDomainEvent(newExtract, oldExtract));
                    }
                    else
                    {
                        await _mediator.Publish(new ExtractUpdateDomainEvent(newExtract, oldExtract));
                    }

                    await _repositoryExtract.UpdateAsync(newExtract);
                }

                return newExtract._id.ToString();
            }
        }
    }
}