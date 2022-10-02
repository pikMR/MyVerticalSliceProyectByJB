using AutoMapper;
using FluentValidation;
using MediatR;
using ServiMotor.Business.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Features.Oils
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
            private readonly IOilRepository _repository;
            private readonly IMapper _mapper;

            public QueryHandler(IOilRepository repository, IMapper mapper)
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
            private readonly IOilRepository _repository;

            public CommandHandler(IOilRepository repository) => _repository = repository;

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                Oil oil;
                if (request.Id == null)
                {
                    oil = new Oil();
                    await _repository.Create(oil);
                }
                else
                {
                    oil = await _repository.Get(request.Id);
                }
                return oil._id.ToString();
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateProjection<Oil, Command>();
            }
        }
    }
}