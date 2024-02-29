﻿using AutoMapper;
using MongoDB.Bson;
using ServiMotor.Business.Models;
using static ServiMotor.Features.Extracts.GetAll;

namespace ServiMotor.Features.Extracts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Extract, Create.Command>();
            CreateMap<Extract, Result.ExtractDto>()
                .ForMember(x => x.Id, src => src.MapFrom(s => s._id.ToString()))
                .ForMember(x => x.Date, src => src.MapFrom(s => s.Date.ToString("yyyy-MM-dd")))
                ;
            CreateMap<Create.Command, Extract>();
            CreateMap<Update.Command, Extract>()
                .ForMember(x => x._id, src => src.MapFrom(s => ObjectId.Parse(s.Id)));
        }
    }
}