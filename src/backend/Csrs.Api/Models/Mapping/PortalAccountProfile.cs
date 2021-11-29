using AutoMapper;
using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Models.Mapping
{
    public class PortalAccountProfile : Profile
    {
        public PortalAccountProfile()
        {
            CreateMap<PortalAccount, SSG_CsrsParty>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => State.Active))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => Status.Active))
                ;

            CreateMap<SSG_CsrsParty, PortalAccount>();
        }
    }
}
