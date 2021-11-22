using AutoMapper;
using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Models.Mapping
{
    public class PortalFeedbackProfile : Profile
    {
        public PortalFeedbackProfile()
        {
            CreateMap<PortalFeedback, SSG_CsrsFeedback>();
        }
    }
}
