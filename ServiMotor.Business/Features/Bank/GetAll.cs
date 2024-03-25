﻿using AutoMapper;
using MediatR;
using ServiMotor.Business.Models;
using ServiMotor.Business.Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Features.Banks
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
            public IEnumerable<BankDto> Banks { get; init; }

            public record BankDto
            {
                public string Id { get; set; }
                public string Name { get; set; }
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IMapper _mapper;
            private readonly IBaseRepository<Bank> _repository;

            public Handler(IBaseRepository<Bank> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var banks = await _repository.GetAllAsync();

                return new Result
                {
                    Banks = _mapper.Map<IEnumerable<Result.BankDto>>(banks)
                };
            }
        }
    }
}
