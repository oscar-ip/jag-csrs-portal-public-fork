namespace Csrs.Api.Models.Dynamics
{
    public partial class SSG_CsrsChild
    {
        public override Guid Key => ChildId ?? Guid.Empty;
    }
}
