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
    public class Create
    {
        public class Command : IRequest<string>
        {
            public string Id { get; set; }
            public string Description { get; set; }
            public string BankName { get; set; }
            public DateTime Date { get; set; }
            public decimal Balance { get; set; }
            public string Detail { get; set; }
            public string BranchOfficeName { get; set; }

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
                RuleFor(m => m.BankName).Length(0, 16);
                RuleFor(m => m.BranchOfficeName).Length(0, 16);
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
                Extract extract;
                if (request.Id == null)
                {
                    var idBank = await _mediator.Send(new Banks.Create.Command()
                    {
                        Name = request.BankName
                    }, cancellationToken);

                    var idBranchOffice = await _mediator.Send(new BranchOffices.Create.Command()
                    {
                        Name = request.BranchOfficeName
                    }, cancellationToken);

                    extract = _mapper.Map<Extract>(request);
                    extract.Bank.SetId(idBank);
                    extract.BranchOffice.SetId(idBranchOffice);

                    await _repositoryExtract.Create(extract);
                }
                else
                {
                    extract = await _repositoryExtract.Get(request.Id);
                }

                return extract._id.ToString();
            }
        }
    }
}