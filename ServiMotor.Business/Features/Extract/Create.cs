using AutoMapper;
using FluentValidation;
using MediatR;
using ServiMotor.Business.Features.DomainEvents;
using ServiMotor.Business.Models;
using ServiMotor.Features.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Features.Extracts
{
    public class Create
    {
        public class Command : IRequest<string>
        {
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
                RuleFor(m => m.Description).NotNull();
                RuleFor(m => m.Date).NotNull().GreaterThan(DateTime.MinValue);
                RuleFor(m => m.Balance).NotNull();
                RuleFor(m => m.Bank).NotNull();
                RuleFor(m => m.BranchOffice).NotNull();
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
                var bank = await _mediator.Send(request.Bank, cancellationToken);
                var branchOffice = await _mediator.Send(request.BranchOffice, cancellationToken);
                var extract = _mapper.Map<Extract>(request);
                extract.BranchOffice = branchOffice;
                extract.Bank = bank;
                await _repositoryExtract.CreateAsync(extract);
                extract.UpdateResult(new ExtractCreateDomainEvent(extract._id, extract.Balance));
                return extract._id.ToString();
            }
        }
    }
}