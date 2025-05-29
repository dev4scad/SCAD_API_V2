using AutoMapper;
using SCAD_API_V2.Application.DTO;
using SCAD_API_V2.Domain.Entities;

namespace SCAD_API_V2.Application.Mapping
{
    public class LicencaProfile : Profile
    {
        public LicencaProfile()
        {
            CreateMap<Licenca, LicencaDto>()
                .ForMember(dest => dest.LicencaId, opt => opt.MapFrom(src => src.LicencaId));
            CreateMap<LicencaDto, Licenca>()
                .ForMember(dest => dest.LicencaId, opt => opt.MapFrom(src => src.LicencaId));
        }
    }
}
