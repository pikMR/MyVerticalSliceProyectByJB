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

namespace ServiMotor.Features.BranchOffices
{
    public class Update
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
                RuleFor(m => m.Id).NotEmpty();
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
                if (request.Id != null)
                {
                    branchOffice = _mapper.Map<BranchOffice>(request);
                    await _repositoryBranchOffice.UpdateAsync(branchOffice);
                }

                return branchOffice._id.ToString();
            }
        }
    }
}