using AutoMapper;
using MongoDB.Bson;
using ServiMotor.Business.Models;
using ServiMotor.Features.Extracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ServiMotor.Features.Banks.GetAll;

namespace ServiMotor.Features.Banks
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // extracts
            CreateMap<Bank, Result.BankDto>()
                .ForMember(x => x.Id, src => src.MapFrom(s => s._id.ToString()));
            CreateMap<Create.Command, Bank>()
                .ForMember(x => x._id, src => src.MapFrom(s => ObjectId.Parse(s.Id)));
            CreateMap<Update.Command, Bank>()
                .ForMember(x => x._id, src => src.MapFrom(s => ObjectId.Parse(s.Id)));
        }
    }
}
