using AutoMapper;
using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Models.Mapping
{
    public class PartyProfile : Profile
    {
        public PartyProfile()
        {            
            // Party -> SSG_CsrsParty
            CreateMap<Party, SSG_CsrsParty>()
                .ForMember(dest => dest.Key, opt => opt.Ignore())
                .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => State.Active))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => GetActiveStatusCode()))
                ;

            // SSG_CsrsParty -> Party
            CreateMap<SSG_CsrsParty, Party>()
                .ForMember(dest => dest.Identity, opt => opt.Ignore())
                .ForMember(dest => dest.BCeIDGuid, opt => opt.Ignore())
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(_ => _.DateOfBirthString))
                ;
        }

        private int GetActiveStatusCode() => SSG_CsrsParty.StatusCodes.FromName(SSG_CsrsParty.Active).Value;
    }
}
