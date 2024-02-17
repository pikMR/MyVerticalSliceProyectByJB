using AutoMapper;
using FluentValidation;
using MediatR;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ServiMotor.Business.Models;
using ServiMotor.Features.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

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
                Extract extract = null;
                if (request.Id != null)
                {
                    var bank = await _mediator.Send(request.Bank, cancellationToken);
                    var branchOffice = await _mediator.Send(request.BranchOffice, cancellationToken);
                    extract = _mapper.Map<Extract>(request);
                    extract.BranchOffice = branchOffice;
                    extract.Bank = bank;
                    await _repositoryExtract.UpdateAsync(extract);
                }

                return extract._id.ToString();
            }
        }
    }
}