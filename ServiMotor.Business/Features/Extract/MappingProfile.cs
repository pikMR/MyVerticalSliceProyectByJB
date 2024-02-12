using AutoMapper;
using MongoDB.Bson;
using ServiMotor.Business.Models;
using static ServiMotor.Features.Extracts.Create;
using static ServiMotor.Features.Extracts.GetAll;

namespace ServiMotor.Features.Extracts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Extract, Create.Command>();
            CreateMap<Extract, Result.Extract>()
                .ForMember(x => x.Id, src => src.MapFrom(s => s._id.ToString()));
            CreateMap<Create.Command, Extract>()
                .ForMember(x => x.BranchOffice, src => src.MapFrom(s => new BranchOffice(s.BranchOfficeName)))
                .ForMember(x => x.Bank, src => src.MapFrom(s => new Bank(s.BankName)));
        }
    }
}