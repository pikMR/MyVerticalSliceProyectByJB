using AutoMapper;
using FluentValidation;
using MediatR;
using ServiMotor.Business.Models;
using ServiMotor.Features.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Features.Banks
{
    public class Create
    {
        public class Command : IRequest<string>
        {
            public string Id { get; set; }
            public string Name { get; set; }

            public Command()
            {
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.Name).NotEmpty();
            }
        }

        public class CommandHandler : IRequestHandler<Command, string>
        {
            private readonly IBaseRepository<Bank> _repositoryExtract;
            private readonly IMapper _mapper;

            public CommandHandler(IBaseRepository<Bank> repository, IMapper mapper)
            {
                _repositoryExtract = repository;
                _mapper = mapper;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                Bank bank = null;
                if (request.Id == null)
                {
                    bank = _mapper.Map<Bank>(request);
                    await _repositoryExtract.Create(bank);
                }
                else
                {
                    bank = await _repositoryExtract.Get(request.Id);
                }

                return bank._id.ToString();
            }
        }
    }
}
