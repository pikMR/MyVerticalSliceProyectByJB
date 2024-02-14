﻿using AutoMapper;
using MediatR;
using ServiMotor.Business.Models;
using ServiMotor.Features.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        }

        public record Result
        {
            public IEnumerable<Extract> Extracts { get; init; }

            public record Extract
            {
                public string Id { get; set; }
                public string Description { get; set; }
                public string BankName { get; set; }
                public DateTime Date { get; set; }
                public decimal Balance { get; set; }
                public string Detail { get; set; }
                public string BranchOfficeName { get; set; }
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
                var extracts = await _repository.Get();

                return new Result
                {
                    Extracts = _mapper.Map<IEnumerable<Result.Extract>>(extracts)
                };
            }
        }
    }
}