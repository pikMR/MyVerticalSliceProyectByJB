using AutoMapper;
using MongoDB.Bson;
using ServiMotor.Business.Models;
using ServiMotor.Features.Extracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ServiMotor.Features.BranchOffices.GetAll;

namespace ServiMotor.Features.BranchOffices
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // extracts
            CreateMap<BranchOffice, Result.BranchOfficeDto>()
                .ForMember(x => x.Id, src => src.MapFrom(s => s._id.ToString()));
            CreateMap<Create.Command, BranchOffice>()
                .ForMember(x => x._id, src => src.MapFrom(s => ObjectId.Parse(s.Id)));
            CreateMap<Update.Command, BranchOffice>()
                .ForMember(x => x._id, src => src.MapFrom(s => ObjectId.Parse(s.Id)));
        }
    }
}
