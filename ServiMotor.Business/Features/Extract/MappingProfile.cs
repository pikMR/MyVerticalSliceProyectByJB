using AutoMapper;
using ServiMotor.Business.Models;
using static ServiMotor.Features.Extracts.GetAll;

namespace ServiMotor.Features.Extracts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Extract, Create.Command>();
            CreateMap<Extract, Result.Extract>();
        }
    }
}