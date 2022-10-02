using AutoMapper;
using MediatR;
using ServiMotor.Business.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Features.Oils
{
    public class GetAll
    {
        private readonly IMediator _mediator;

        public GetAll(IMediator mediator) => _mediator = mediator;

        public Result Data { get; private set; }

        public async Task OnGetAsync() => Data = await _mediator.Send(new Query());

        public record Query : IRequest<Result>
        {
        }

        public record Result
        {
            public IEnumerable<Oil> Productos { get; init; }

            public record Oil
            {
                public string _id { get; init; }
                public string Name { get; init; }
                public string Unit { get; init; }
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IMapper _mapper;
            private readonly IOilRepository _repository;

            public Handler(IOilRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var productos = (await _repository.Get());

                return new Result
                {
                    Productos = _mapper.Map<IEnumerable<Result.Oil>>(productos)
                };
            }
        }
    }
}