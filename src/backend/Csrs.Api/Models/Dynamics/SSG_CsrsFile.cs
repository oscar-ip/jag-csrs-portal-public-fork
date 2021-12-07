namespace Csrs.Api.Models.Dynamics
{
    public partial class SSG_CsrsFile
    {
        public static readonly StatusCode<FileStatus> Active = new StatusCode<FileStatus>(1, "Active", FileStatus.Active);
        public static readonly StatusCode<FileStatus> Draft = new StatusCode<FileStatus>(867670025, "Draft", FileStatus.Draft);
    }
}
