﻿using FluentValidation;
using MediatR;
using ServiMotor.Business.Features.DomainEvents;
using ServiMotor.Business.Models;
using ServiMotor.Features.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Features.Extracts
{
    public class Delete
    {
        public class Command : IRequest<string>
        {
            public string Id { get; set; }

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

            public CommandHandler(IBaseRepository<Extract> repository)
            {
                _repositoryExtract = repository;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.Id != null)
                {
                    var extract = await _repositoryExtract.GetFirstAsync(x => x._id.ToString() == request.Id);
                    extract.UpdateResult(new ExtractDeleteDomainEvent(extract._id, extract.Balance));
                    await _repositoryExtract.DeleteAsync(request.Id);
                }

                return request.Id.ToString();
            }
        }
    }
}