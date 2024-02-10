using AutoMapper;
using FluentValidation;
using MediatR;
using ServiMotor.Business.Models;
using ServiMotor.Features.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Features.Extracts
{
    public class Create
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
            public string Id { get; init; }
            public string Name { get; init; }
            public string Unit { get; init; }

            public Command()
            {
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.Unit).NotNull().Length(0, 8);
                RuleFor(m => m.Name).NotNull();
            }
        }

        public class CommandHandler : IRequestHandler<Command, string>
        {
            private readonly IBaseRepository<Extract> _repository;

            public CommandHandler(IBaseRepository<Extract> repository) => _repository = repository;

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                Extract extract;
                if (request.Id == null)
                {
                    extract = new Extract();
                    await _repository.Create(extract);
                }
                else
                {
                    extract = await _repository.Get(request.Id);
                }
                return extract._id.ToString();
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateProjection<Extract, Command>();
            }
        }
    }
}