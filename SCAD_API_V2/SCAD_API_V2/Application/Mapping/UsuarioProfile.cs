using AutoMapper;
using SCAD_API_V2.Application.DTO;
using SCAD_API_V2.Domain.Entities;

namespace SCAD_API_V2.Application.Mapping
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId));
            CreateMap<UsuarioDto, Usuario>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId));
        }
    }
}
