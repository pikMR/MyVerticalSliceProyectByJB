using AutoMapper;
using MediatR;
using ServiMotor.Business.Models;
using ServiMotor.Features.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using static ServiMotor.Features.Banks.GetAll.Result;
using static ServiMotor.Features.BranchOffices.GetAll.Result;

namespace ServiMotor.Features.Extracts
{
    public class GetAll
    {
        private readonly IMediator _mediator;

        public GetAll(IMediator mediator) => _mediator = mediator;

        public Result Data { get; private set; }

        public async Task OnGetAsync() => Data = await _mediator.Send(new Query());

        public record Query : IRequest<Result>
        {
            public Expression<Func<Extract,bool>> Filter { get; set; }
        }

        public record Result
        {
            public IEnumerable<ExtractDto> Extracts { get; init; }

            public record ExtractDto
            {
                public string Id { get; set; }
                public string Description { get; set; }
                public BankDto Bank { get; set; }
                public string Date { get; set; }
                public decimal Balance { get; set; }
                public string Detail { get; set; }
                public BranchOfficeDto BranchOffice { get; set; }
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IMapper _mapper;
            private readonly IBaseRepository<Extract> _repository;

            public Handler(IBaseRepository<Extract> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                IEnumerable<Extract> extracts;

                if(request.Filter != null)
                {
                    extracts = await _repository.FindAsync(request.Filter, cancellationToken);
                }
                else
                {
                    extracts = await _repository.Get();
                }
                
                return new Result
                {
                    Extracts = _mapper.Map<IEnumerable<Result.ExtractDto>>(extracts)
                };
            }
        }
    }
}