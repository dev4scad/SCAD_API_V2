using AutoMapper;
using SCAD_API_V2.Application.DTO;
using SCAD_API_V2.Domain.Entities;

namespace SCAD_API_V2.Application.Mapping
{
    public class VinculoProfile : Profile
    {
        public VinculoProfile()
        {
            CreateMap<Vinculo, VinculoDto>()
                .ForMember(dest => dest.VinculoId, opt => opt.MapFrom(src => src.VinculoId));
            CreateMap<VinculoDto, Vinculo>()
                .ForMember(dest => dest.VinculoId, opt => opt.MapFrom(src => src.VinculoId));
        }
    }
}
