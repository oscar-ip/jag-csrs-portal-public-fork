using AutoMapper;
using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Models.Mapping
{
    public class PortalFileProfile : Profile
    {
        public PortalFileProfile()
        {
            CreateMap<PortalFile, SSG_CsrsFile>();
            CreateMap<SSG_CsrsFile, PortalFile>();
        }
    }
}
