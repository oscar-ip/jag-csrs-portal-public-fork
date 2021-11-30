using AutoMapper;
using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Models.Mapping
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
            CreateMap<File, SSG_CsrsFile>();
            CreateMap<SSG_CsrsFile, File>();
        }
    }
}
