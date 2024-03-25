using AutoMapper;
using FluentValidation;
using MediatR;
using ServiMotor.Business.Models;
using ServiMotor.Business.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiMotor.Features.Banks
{
    public class Create
    {
        public class Command : IRequest<Bank>
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

        public class CommandHandler : IRequestHandler<Command, Bank>
        {
            private readonly IBaseRepository<Bank> _repository;
            private readonly IMapper _mapper;

            public CommandHandler(IBaseRepository<Bank> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Bank> Handle(Command request, CancellationToken cancellationToken)
            {
                var idBank = request.Id;
                Bank bank;

                if (idBank == null)
                {
                    bank = await _repository.GetFirstAsync(x => x.Name.Equals(request.Name.Trim()));
                    if (bank == null)
                    {
                        bank = _mapper.Map<Bank>(request);
                        await _repository.CreateAsync(bank);
                    }

                    return bank;
                }
                else
                {
                    bank = await _repository.GetAsync(idBank);

                    if (bank == null)
                    {
                        throw new ArgumentException($"Bank with id {request.Id} not exist");
                    }
                }

                return bank;
            }
        }
    }
}
