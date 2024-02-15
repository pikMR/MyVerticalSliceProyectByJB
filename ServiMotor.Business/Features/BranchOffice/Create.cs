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

namespace ServiMotor.Features.BranchOffices
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
            private readonly IBaseRepository<BranchOffice> _repositoryBranchOffice;
            private readonly IMapper _mapper;

            public CommandHandler(IBaseRepository<BranchOffice> repository, IMapper mapper)
            {
                _repositoryBranchOffice = repository;
                _mapper = mapper;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                BranchOffice branchOffice = null;
                if (request.Id == null)
                {
                    branchOffice = _mapper.Map<BranchOffice>(request);
                    await _repositoryBranchOffice.Create(branchOffice);
                }
                else
                {
                    branchOffice = await _repositoryBranchOffice.Get(request.Id);
                }

                return branchOffice._id.ToString();
            }
        }
    }
}
