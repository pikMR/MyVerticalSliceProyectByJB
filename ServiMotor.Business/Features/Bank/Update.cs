﻿using AutoMapper;
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

namespace ServiMotor.Features.Banks
{
    public class Update
    {
        public class Command : IRequest<string>
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
                RuleFor(m => m.Id).NotEmpty();
                RuleFor(m => m.Name).NotEmpty();
            }
        }

        public class CommandHandler : IRequestHandler<Command, string>
        {
            private readonly IBaseRepository<Bank> _repositoryBank;
            private readonly IMapper _mapper;

            public CommandHandler(IBaseRepository<Bank> repository, IMapper mapper)
            {
                _repositoryBank = repository;
                _mapper = mapper;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                Bank bank = null;
                if (request.Id != null)
                {
                    bank = _mapper.Map<Bank>(request);
                    await _repositoryBank.UpdateAsync(bank);
                }

                return bank._id.ToString();
            }
        }
    }
}