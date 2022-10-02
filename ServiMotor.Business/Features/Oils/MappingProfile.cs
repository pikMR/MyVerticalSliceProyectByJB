using AutoMapper;
using ServiMotor.Business.Models;
using static ServiMotor.Features.Oils.GetAll;

namespace ServiMotor.Features.Oils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Oil, Create.Command>();
            CreateMap<Oil, Result.Oil>();
        }
    }
}