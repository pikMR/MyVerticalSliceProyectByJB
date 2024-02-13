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
        public class Query : IRequest<Command>
        {
            public string Id { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly IBaseRepository<Extract> _repository;
            private readonly IMapper _mapper;

            public QueryHandler(IBaseRepository<Extract> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                Command model;
                if (request.Id == null)
                {
                    model = new Command();
                }
                else
                {
                    model = _mapper.Map<Command>(await _repository.Get(request.Id));
                }
                return model;
            }
        }

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
                RuleFor(m => m.Id).NotEmpty();
            }
        }

        public class CommandHandler : IRequestHandler<Command, string>
        {
            private readonly IBaseRepository<Extract> _repositoryExtract;
            private readonly IMapper _mapper;

            public CommandHandler(IBaseRepository<Extract> repository, IMapper mapper)
            {
                _repositoryExtract = repository;
                _mapper = mapper;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                Extract extract = null;
                if (request.Id != null)
                {
                    extract = _mapper.Map<Extract>(request);
                    _repositoryExtract.Update(extract);
                }

                return extract._id.ToString();
            }
        }
    }
}