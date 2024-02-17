using AutoMapper;
using FluentValidation;
using MediatR;
using MongoDB.Bson;
using ServiMotor.Business.Models;
using ServiMotor.Features.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ServiMotor.Features.Banks.GetAll.Result;

namespace ServiMotor.Features.BranchOffices
{
    public class Create
    {
        public class Command : IRequest<BranchOffice>
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

        public class CommandHandler : IRequestHandler<Command, BranchOffice>
        {
            private readonly IBaseRepository<BranchOffice> _repository;
            private readonly IMapper _mapper;

            public CommandHandler(IBaseRepository<BranchOffice> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<BranchOffice> Handle(Command request, CancellationToken cancellationToken)
            {
                var idBranchOffice = request.Id;
                BranchOffice branchOffice;

                if (idBranchOffice == null)
                {
                    branchOffice = await _repository.GetFirstAsync(x => x.Name.Equals(request.Name.Trim()));
                    if (branchOffice == null)
                    {
                        branchOffice = _mapper.Map<BranchOffice>(request);
                        await _repository.Create(branchOffice);
                    }

                    return branchOffice;
                }
                else
                {
                    var id = ObjectId.Parse(idBranchOffice);
                    branchOffice = await _repository.GetFirstAsync(x => x._id.Equals(id));

                    if (branchOffice == null)
                    {
                        throw new ArgumentException($"BranchOffice with id {id} not exist");
                    }
                }

                return branchOffice;
            }
        }
    }
}
