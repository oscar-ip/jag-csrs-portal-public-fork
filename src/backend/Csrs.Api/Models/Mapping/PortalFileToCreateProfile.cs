using AutoMapper;
using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Models.Mapping
{
    public class PortalFileToCreateProfile : Profile
    {
        public PortalFileToCreateProfile()
        {
            CreateMap<PortalFile, SSG_CsrsFile>()
                .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => State.Active))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => Status.Active))
                ;
        }
    }
}
