using AutoMapper;
using SCAD_API_V2.Application.DTO;
using SCAD_API_V2.Domain.Entities;

namespace SCAD_API_V2.Application.Mapping
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<Cliente, ClienteDto>()
                .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.ClienteId));
            CreateMap<ClienteDto, Cliente>()
                .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.ClienteId));
        }
    }
}
